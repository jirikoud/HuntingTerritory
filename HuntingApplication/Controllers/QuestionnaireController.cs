using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.ViewModel.QuestionnaireModels;
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
    public class QuestionnaireController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Questionnaire/id
        public ActionResult Index(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItemType = MapItemTypeContext.GetDetail(dataContext, id);
                    if (mapItemType == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(mapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = mapItemType.TerritoryId });
                    }

                    var itemList = QuestionnaireContext.GetList(dataContext, mapItemType);
                    var model = new QuestionnaireListModel(mapItemType, itemList);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Questionnaire/Create
        public ActionResult Create(int mapItemTypeId)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItemType = MapItemTypeContext.GetDetail(dataContext, mapItemTypeId);
                    if (mapItemType == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(mapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = mapItemType.TerritoryId });
                    }
                    var model = new QuestionnaireUpdateModel()
                    {
                        IsCreate = true,
                        MapItemTypeId = mapItemTypeId,
                    };
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questionnaire/Create
        [HttpPost]
        public ActionResult Create(QuestionnaireUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItemType = MapItemTypeContext.GetDetail(dataContext, model.MapItemTypeId);
                    if (mapItemType == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(mapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = mapItemType.TerritoryId });
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = QuestionnaireContext.Update(dataContext, null, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionnaireRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "Questionnaire", new { id = model.MapItemTypeId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionnaireRes.ERROR_CREATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Questionnaire/Update/id
        public ActionResult Update(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, id);
                    if (questionnaire == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = questionnaire.MapItemType.TerritoryId });
                    }
                    var model = new QuestionnaireUpdateModel(questionnaire);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questionnaire/Update
        [HttpPost]
        public ActionResult Update(QuestionnaireUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, model.Id);
                    if (questionnaire == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = questionnaire.MapItemType.TerritoryId });
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = QuestionnaireContext.Update(dataContext, questionnaire, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionnaireRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "Questionnaire", new { id = model.MapItemTypeId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionnaireRes.ERROR_UPDATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questionnaire/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, id);
                    if (questionnaire == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = questionnaire.MapItemType.TerritoryId });
                    }

                    var isSuccess = QuestionnaireContext.Delete(dataContext, questionnaire, user);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionnaireRes.SUCCESS_DELETE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionnaireRes.ERROR_DELETE);
                    }
                    return RedirectToAction("Index", "Questionnaire", new { id = questionnaire.MapItemTypeId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionnaireController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}