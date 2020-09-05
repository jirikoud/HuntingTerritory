using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.ViewModel.UserPointModels;
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
    [AllowGoogleMap]
    public class UserPointController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: UserPoint
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserPoint/Create
        public ActionResult Create(int territoryId)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;

                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var territory = TerritoryContext.GetDetail(dataContext, territoryId);
                    if (user.CanViewTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new UserPointUpdateModel()
                    {
                        IsCreate = true,
                        TerritoryId = territoryId,
                    };
                    model.FillTerritoryInfo(territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UserPointController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserPoint/Create
        [HttpPost]
        public ActionResult Create(UserPointUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;

                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var territory = TerritoryContext.GetDetail(dataContext, model.TerritoryId);
                    if (user.CanViewTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    model.Validate(ModelState);
                    if (ModelState.IsValid)
                    {
                        var itemId = UserPointContext.Update(dataContext, null, model, user.Id);
                        if (itemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, UserPointRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "Territory", new { id = model.TerritoryId });
                        }
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, UserPointRes.ERROR_CREATE);
                    }

                    model.FillTerritoryInfo(territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UserPointController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        //GET: UserPoint/Update/id
        public ActionResult Update(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;

                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var userPoint = UserPointContext.GetUserPoint(dataContext, id);
                    if (userPoint == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateUserPoint(userPoint) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new UserPointUpdateModel(userPoint);
                    model.FillTerritoryInfo(userPoint.Territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UserPointController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: UserPoint/Update
        [HttpPost]
        public ActionResult Update(UserPointUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var userName = User.Identity.Name;

                    var user = AclUserContext.GetDetail(dataContext, userName);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var userPoint = UserPointContext.GetUserPoint(dataContext, model.Id);
                    if (userPoint == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateUserPoint(userPoint) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    model.Validate(ModelState);
                    if (ModelState.IsValid)
                    {
                        var itemId = UserPointContext.Update(dataContext, userPoint, model, user.Id);
                        if (itemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, UserPointRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "Territory", new { id = model.TerritoryId });
                        }
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, UserPointRes.ERROR_UPDATE);
                    }

                    model.FillTerritoryInfo(userPoint.Territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UserPointController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}