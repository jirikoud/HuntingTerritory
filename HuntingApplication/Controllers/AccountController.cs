using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HuntingModel.ViewModel.AccountModels;
using NLog;
using HuntingModel.Context;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingApplication.Infrastructure;
using System.Web.Security;
using HuntingModel.Database;

namespace HuntingApplication.Controllers
{
    [Authorize]
    [Localization]
    public class AccountController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    ViewBag.ReturnUrl = returnUrl;
                    var model = new LoginModel()
                    {
                        CanDemo = dataContext.AclUsers.Any(item => item.IsDeleted == false && item.AccountType == (int)AccountTypeEnum.Demo),
                    };
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }

                    UserSession userSession;
                    var result = AclUserContext.LoginUser(dataContext, model.Email, model.Password, out userSession);
                    switch (result)
                    {
                        case LoginResultEnum.Success:
                            FormsAuthentication.SetAuthCookie(userSession.AclUser.Email, model.RememberMe);
                            return RedirectToLocal(returnUrl);
                        case LoginResultEnum.NotAllowed:
                            return View("Lockout");
                        case LoginResultEnum.Error:
                            return View("LoginError");
                        default:
                            ModelState.AddModelError("", AccountRes.VALIDATION_LOGIN_INVALID);
                            return View(model);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult DemoLogin(string returnUrl)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    UserSession userSession;
                    var demoUser = dataContext.AclUsers.FirstOrDefault(item => item.IsDeleted == false && item.AccountType == (int)AccountTypeEnum.Demo);
                    var result = AclUserContext.LoginDemoUser(dataContext, demoUser, out userSession);
                    if (result)
                    {
                        FormsAuthentication.SetAuthCookie(userSession.AclUser.Email, false);
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.SUCCESS_LOGIN_DEMO);
                        return RedirectToLocal(returnUrl);
                    }
                    ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_LOGIN_DEMO);
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return RedirectToAction("Index", "Home");
            //return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            return RedirectToAction("Index", "Home");
            //try
            //{
            //    using (var dataContext = new HuntingEntities())
            //    {
            //        if (ModelState.IsValid)
            //        {
            //            UserSession userSession;
            //            var result = AclUserContext.RegisterUser(dataContext, model, out userSession);
            //            switch (result)
            //            {
            //                case RegisterResultEnum.Error:
            //                    return View("RegisterError");
            //                case RegisterResultEnum.AlreadyUsed:
            //                    ModelState.AddModelError("", AccountRes.VALIDATION_REGISTER_USED);
            //                    break;
            //                case RegisterResultEnum.Success:
            //                    FormsAuthentication.SetAuthCookie(userSession.AclUser.Email, false);
            //                    return RedirectToAction("Index", "Home");
            //            }
            //        }
            //        return View(model);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    logger.Error(exception, "AccountController");
            //    return RedirectToAction("Index", "Home");
            //}
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult LanguageSelection()
        {
            try
            {
                int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                var languageList = LanguageContext.GetLanguageList();
                languageList = languageList.Where(item => item.Id != languageId).ToList();
                var model = new LanguageListModel(languageList);
                return PartialView(model);
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                return PartialView(@"AjaxError", string.Empty);
            }
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult AdminMenu()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    if (Request.IsAuthenticated)
                    {
                        var userName = User.Identity.Name;
                        var user = AclUserContext.GetDetail(dataContext, userName);
                        var isAdmin = user.AccountTypeEx == AccountTypeEnum.Admin;
                        return PartialView(isAdmin);
                    }
                    return PartialView(false);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                return PartialView(@"AjaxError", string.Empty);
            }

        }

        [AllowAnonymous]
        public ActionResult Confirm(string code)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var aclUser = AclUserContext.GetDetailByEmailCode(dataContext, code);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_CODE_INVALID);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangePasswordModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var aclUser = AclUserContext.GetDetailByEmailCode(dataContext, model.Code);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_CODE_INVALID);
                        return RedirectToAction("Index", "Home");
                    }
                    if (ModelState.IsValid)
                    {
                        var isSuccess = AclUserContext.ChangePassword(dataContext, aclUser, model, true);
                        if (isSuccess)
                        {
                            FormsAuthentication.SetAuthCookie(aclUser.Email, false);
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AccountRes.SUCCESS_CONFIRM);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Manage()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var aclUser = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new ManageModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ChangePassword()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var aclUser = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var aclUser = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    if (ModelState.IsValid)
                    {
                        var isSuccess = AclUserContext.ChangePassword(dataContext, aclUser, model, false);
                        if (isSuccess)
                        {
                            FormsAuthentication.SetAuthCookie(aclUser.Email, false);
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AccountRes.SUCCESS_PASSWORD_CHANGED);
                            return RedirectToAction("Manage", "Account");
                        }
                    }
                    model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new ForgotPasswordModel();
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgottenPassword(ForgotPasswordModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    if (ModelState.IsValid)
                    {
                        var aclUser = AclUserContext.GetDetail(dataContext, model.Email);
                        if (aclUser != null)
                        {
                            if (aclUser.AccountTypeEx == AccountTypeEnum.Admin)
                            {
                                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_FORGOTTEN_PASSWORD_ADMIN);
                                return RedirectToAction("Index", "Home");
                            }
                            var isSuccess = AclUserContext.ForgotPassword(dataContext, aclUser);
                            if (isSuccess)
                            {
                                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AccountRes.SUCCESS_FORGOTTEN_PASSWORD);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AccountRes.SUCCESS_FORGOTTEN_PASSWORD);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgottenPasswordConfirm(string code)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var aclUser = AclUserContext.GetDetailByEmailCode(dataContext, code);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_CODE_INVALID);
                        return RedirectToAction("Index", "Home");
                    }
                    if (aclUser.AccountTypeEx == AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_FORGOTTEN_PASSWORD_ADMIN);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPasswordConfirm(ChangePasswordModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    if (string.IsNullOrWhiteSpace(User.Identity.Name) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var aclUser = AclUserContext.GetDetailByEmailCode(dataContext, model.Code);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_CODE_INVALID);
                        return RedirectToAction("Index", "Home");
                    }
                    if (aclUser.AccountTypeEx == AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AccountRes.ERROR_FORGOTTEN_PASSWORD_ADMIN);
                        return RedirectToAction("Index", "Home");
                    }
                    if (ModelState.IsValid)
                    {
                        var isSuccess = AclUserContext.ChangePassword(dataContext, aclUser, model, false);
                        if (isSuccess)
                        {
                            FormsAuthentication.SetAuthCookie(aclUser.Email, false);
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AccountRes.SUCCESS_PASSWORD_CHANGED);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    model = new ChangePasswordModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AccountController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

    }
}