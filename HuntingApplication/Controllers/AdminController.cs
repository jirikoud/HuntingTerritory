using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.SqlGenerator.Generators;
using HuntingModel.ViewModel.AdminModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuntingApplication.Controllers
{
    [Authorize]
    [Localization]
    public class AdminController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin
        public ActionResult Index(AclUserFilter filter, int? page)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    filter.PrepareFilter(languageId);
                    int pageIndex = page ?? 0;
                    var itemList = AclUserContext.GetList(dataContext, filter, pageIndex);
                    var itemListModel = new AclUserListModel(itemList, pageIndex);
                    var model = new AclUserPageModel(itemListModel, filter);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: UserDetail/id
        public ActionResult UserDetail(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    var aclUser = AclUserContext.GetDetail(dataContext, id);
                    var model = new AclUserDetailModel(aclUser);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: UserUpdate/id
        public ActionResult UserUpdate(int? id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    AclUserUpdateModel model;
                    if (id.HasValue)
                    {
                        var aclUser = AclUserContext.GetDetail(dataContext, id.Value);
                        model = new AclUserUpdateModel(aclUser);
                    }
                    else
                    {
                        model = new AclUserUpdateModel()
                        {
                            IsCreate = true,
                        };
                    }
                    model.FillCodeLists(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserUpdate/id
        [HttpPost]
        public ActionResult UserUpdate(AclUserUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    AclUser updateItem = null;
                    if (model.IsCreate == false)
                    {
                        updateItem = AclUserContext.GetDetail(dataContext, model.Id);
                        if (updateItem == null)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        var isUsed = AclUserContext.IsEmailUsed(dataContext, model.Email, model.Id);
                        if (isUsed)
                        {
                            ModelState.AddModelError("Email", AdminRes.VALIDATION_EMAIL_USED);
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        var itemId = AclUserContext.Update(dataContext, updateItem, model, user.Id);
                        if (itemId.HasValue)
                        {
                            return RedirectToAction("UserDetail", "Admin", new { id = itemId.Value });
                        }
                        ModelState.AddModelError("", GlobalRes.VALIDATION_UPDATE_FAILED);
                    }
                    model.FillCodeLists(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserDelete/id
        [HttpPost]
        public ActionResult UserDelete(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var aclUser = AclUserContext.GetDetail(dataContext, id);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Admin");
                    }

                    var isSuccess = AclUserContext.Delete(dataContext, aclUser, user.Id);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AdminRes.SUCCESS_USER_DELETE);
                        return RedirectToAction("Index", "Admin");
                    }
                    ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AdminRes.ERROR_USER_DELETE);
                    return RedirectToAction("UserDetail", "Admin", new { id = id });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserDisable/id
        [HttpPost]
        public ActionResult UserDisable(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var aclUser = AclUserContext.GetDetail(dataContext, id);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Admin");
                    }

                    var isSuccess = AclUserContext.Disable(dataContext, aclUser, user.Id, true);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AdminRes.SUCCESS_USER_DISABLE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AdminRes.ERROR_USER_DISABLE);
                    }
                    return RedirectToAction("UserDetail", "Admin", new { id = id });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserEnable/id
        [HttpPost]
        public ActionResult UserEnable(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;
                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user.AccountTypeEx != AccountTypeEnum.Admin)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var aclUser = AclUserContext.GetDetail(dataContext, id);
                    if (aclUser == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Admin");
                    }

                    var isSuccess = AclUserContext.Disable(dataContext, aclUser, user.Id, false);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, AdminRes.SUCCESS_USER_ENABLE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, AdminRes.ERROR_USER_ENABLE);
                    }
                    return RedirectToAction("UserDetail", "Admin", new { id = id });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "AdminController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}