using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;
using HuntingModel.Enumeration;
using HuntingModel.ViewModel.AccountModels;
using HuntingModel.Infrastructure;
using HuntingModel.Properties;
using HuntingModel.SqlGenerator.Generators;
using HuntingModel.SqlGenerator;
using HuntingModel.ViewModel.AdminModels;
using HuntingModel.Database;
using System.Web.Mvc;

namespace HuntingModel.Context
{
    public static class AclUserContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private const int SESSION_EXPIRE_MINUTES = 30*24*60;

        public static AclUser CreateAdmin()
        {
            using (var dataContext = new HuntingEntities())
            {
                var adminUser = dataContext.AclUsers.FirstOrDefault(item => item.Email == Settings.Default.AdminEmail);
                if (adminUser == null)
                {
                    adminUser = new AclUser()
                    {
                        AccountTypeEx = AccountTypeEnum.Admin,
                        Email = Settings.Default.AdminEmail,
                        PasswordHash = PasswordStorage.CreateHash(Settings.Default.AdminPassword),
                        SysCreated = DateTime.Now,
                        MaxTerritoryCount = -1,
                        Fullname = Settings.Default.AdminFullname,
                    };
                    dataContext.AclUsers.Add(adminUser);
                }
                else
                {
                    adminUser.AccountTypeEx = AccountTypeEnum.Admin;
                    adminUser.PasswordHash = PasswordStorage.CreateHash(Settings.Default.AdminPassword);
                }
                dataContext.SaveChanges();
                return adminUser;
            }
        }

        public static AclUser GetAdminAccount(HuntingEntities dataContext)
        {
            var adminUser = dataContext.AclUsers.FirstOrDefault(item => item.Email == Settings.Default.AdminEmail);
            if (adminUser == null)
            {
                return CreateAdmin();
            }
            return adminUser;
        }

        public static AclUser GetDetail(HuntingEntities dataContext, string userName)
        {
            try
            {
                var lowerUserName = userName.Trim().ToLower();
                var user = dataContext.AclUsers.FirstOrDefault(item => item.Email == lowerUserName && item.IsDeleted == false);
                return user;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
            }
            return null;
        }

        public static AclUser GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var user = dataContext.AclUsers.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return user;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
            }
            return null;
        }

        public static AclUser GetDetailByEmailCode(HuntingEntities dataContext, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }
            try
            {
                var user = dataContext.AclUsers.FirstOrDefault(item => item.EmailCode == code && item.IsDeleted == false);
                return user;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetailByEmailCode");
            }
            return null;
        }

        public static LoginResultEnum LoginUser(HuntingEntities dataContext, string email, string password, out UserSession session)
        {
            session = null;
            try
            {
                var lowerEmail = email.Trim().ToLower();
                var foundUser = dataContext.AclUsers.FirstOrDefault(item => item.IsDeleted == false && item.Email == lowerEmail);
                if (foundUser != null)
                {
                    var isValidPassword = PasswordStorage.VerifyPassword(password, foundUser.PasswordHash);
                    if (isValidPassword == false)
                    {
                        return LoginResultEnum.WrongPassword;
                    }
                    if (foundUser.IsDisabled)
                    {
                        return LoginResultEnum.NotAllowed;
                    }
                    var newSession = new UserSession()
                    {
                        AclUserId = foundUser.Id,
                        SysCreated = DateTime.Now,
                        Session = Guid.NewGuid().ToString(),
                    };
                    dataContext.UserSessions.Add(newSession);
                    dataContext.SaveChanges();
                    session = newSession;
                    return LoginResultEnum.Success;
                }
                return LoginResultEnum.NotFound;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "LoginUser({0},{1})", email, password);
                return LoginResultEnum.Error;
            }
        }

        public static bool LoginDemoUser(HuntingEntities dataContext, AclUser demoUser, out UserSession session)
        {
            session = null;
            try
            {
                var newSession = new UserSession()
                {
                    AclUserId = demoUser.Id,
                    SysCreated = DateTime.Now,
                    Session = Guid.NewGuid().ToString(),
                };
                dataContext.UserSessions.Add(newSession);
                dataContext.SaveChanges();
                session = newSession;
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "LoginDemoUser");
                return false;
            }
        }

        public static RegisterResultEnum RegisterUser(HuntingEntities dataContext, RegisterModel model, out UserSession session)
        {
            session = null;
            try
            {
                var lowerEmail = model.Email.Trim().ToLower();
                var isAny = dataContext.AclUsers.Any(item => item.Email == lowerEmail);
                if (isAny)
                {
                    return RegisterResultEnum.AlreadyUsed;
                }
                var newUser = new AclUser()
                {
                    Email = model.Email,
                    PasswordHash = PasswordStorage.CreateHash(model.Password),
                    SysCreated = DateTime.Now,
                };
                dataContext.AclUsers.Add(newUser);
                var newSession = new UserSession()
                {
                    AclUser = newUser,
                    SysCreated = DateTime.Now,
                    Session = Guid.NewGuid().ToString(),
                };
                dataContext.UserSessions.Add(newSession);
                dataContext.SaveChanges();
                session = newSession;
                return RegisterResultEnum.Success;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "RegisterUser");
                return RegisterResultEnum.Error;
            }
        }

        public static bool ChangePassword(HuntingEntities dataContext, AclUser aclUser, ChangePasswordModel model, bool isConfirm)
        {
            try
            {
                aclUser.EmailCode = null;
                aclUser.EmailCodeExpire = null;
                aclUser.PasswordHash = PasswordStorage.CreateHash(model.Password);
                var newSession = new UserSession()
                {
                    AclUser = aclUser,
                    SysCreated = DateTime.Now,
                    Session = Guid.NewGuid().ToString(),
                };
                dataContext.UserSessions.Add(newSession);
                if (isConfirm)
                {
                    foreach (var territoryUser in aclUser.TerritoryUsers)
                    {
                        if (territoryUser.UserRoleEx == TerritoryUserRoleEnum.Invited)
                        {
                            territoryUser.UserRoleEx = TerritoryUserRoleEnum.Member;
                        }
                    }
                }

                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "ChangePassword");
                return false;
            }
        }

        public static int GetTotalCount(HuntingEntities dataContext, AclUserFilter filter)
        {
            try
            {
                var generator = new AclUserListGenerator(filter);
                var totalCount = QueryGenerator.GetTotalCount(dataContext, generator);
                return totalCount;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTotalCount");
                return 0;
            }
        }

        public static ItemListHolder<AclUserListItem> GetList(HuntingEntities dataContext, AclUserFilter filter, int pageIndex)
        {
            try
            {
                var generator = new AclUserListGenerator(filter);
                var list = QueryGenerator.ReadPagedList<AclUserListItem>(dataContext, generator, pageIndex, Constants.DEFAULT_LIST_PAGE_SIZE);
                var itemHolder = new ItemListHolder<AclUserListItem>(list);
                itemHolder.TotalCount = GetTotalCount(dataContext, filter);
                return itemHolder;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetList");
                return new ItemListHolder<AclUserListItem>();
            }
        }

        public static bool IsEmailUsed(HuntingEntities dataContext, string email, int? userId)
        {
            var isUsed = dataContext.AclUsers.Any(item => item.Email == email && item.Id != userId);
            return isUsed;
        }

        public static int? Update(HuntingEntities dataContext, AclUser updateItem, AclUserUpdateModel model, int userId)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new AclUser()
                    {
                        SysCreated = DateTime.Now,
                        EmailCode = Guid.NewGuid().ToString(),
                    };
                    dataContext.AclUsers.Add(updateItem);
                }
                updateItem.Email = model.Email;
                updateItem.AccountTypeEx = model.AccountType;
                updateItem.Fullname = model.Fullname;
                if (updateItem.AccountTypeEx == AccountTypeEnum.Admin)
                {
                    updateItem.MaxTerritoryCount = -1;
                }
                else if (updateItem.AccountTypeEx == AccountTypeEnum.Standard)
                {
                    updateItem.MaxTerritoryCount = 0;
                }
                else
                {
                    updateItem.MaxTerritoryCount = int.Parse(model.MaxTerritoryCount);
                }
                updateItem.SysEditor = userId;
                updateItem.SysUpdated = DateTime.Now;
                if (model.IsCreate)
                {
                    EmailContext.CreateRegistrationEmail(dataContext, updateItem, userId);
                }
                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Update");
            }
            return null;
        }

        public static bool Delete(HuntingEntities dataContext, AclUser aclUser, int userId)
        {
            try
            {
                aclUser.SysEditor = userId;
                aclUser.SysUpdated = DateTime.Now;
                aclUser.IsDeleted = true;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Delete");
            }
            return false;
        }

        public static bool Disable(HuntingEntities dataContext, AclUser aclUser, int userId, bool isDisabled)
        {
            try
            {
                aclUser.SysEditor = userId;
                aclUser.SysUpdated = DateTime.Now;
                aclUser.IsDisabled = isDisabled;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Disable({0})", isDisabled);
            }
            return false;
        }

        public static bool ForgotPassword(HuntingEntities dataContext, AclUser aclUser)
        {
            try
            {
                aclUser.EmailCode = Guid.NewGuid().ToString();
                aclUser.EmailCodeExpire = DateTime.Now.AddHours(Settings.Default.EmailCodeExpireHours);
                EmailContext.CreateForgottenEmail(dataContext, aclUser);
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "ForgotPassword({0})", aclUser != null ? aclUser.Email : "N/A");
                return false;
            }
        }

        public static void ClearExpiredEmailCodes(HuntingEntities dataContext)
        {
            try
            {
                var aclUserList = dataContext.AclUsers.Where(item => item.EmailCodeExpire < DateTime.Now);
                foreach (var aclUser in aclUserList)
                {
                    aclUser.EmailCode = null;
                    aclUser.EmailCodeExpire = null;
                }
                dataContext.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "ClearExpiredEmailCodes");
            }
        }

        public static UserSession GetUserSession(HuntingEntities dataContext, string session)
        {
            try
            {
                var expireDate = DateTime.Now.AddMinutes(-SESSION_EXPIRE_MINUTES);

                var userSession = dataContext.UserSessions.FirstOrDefault(item => 
                    item.IsDeleted == false && item.Session == session && item.SysCreated > expireDate);
                return userSession;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetUserSession({0})", session);
                return null;
            }
        }

    }
}
