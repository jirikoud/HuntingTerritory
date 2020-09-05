using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.SqlGenerator.Generators;
using HuntingModel.ViewModel.CheckInModels;
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
    public class CheckInController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: CheckIn/id
        public ActionResult Index(int id, CheckInFilter filter, int? page)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItem = MapItemContext.GetDetail(dataContext, id);
                    if (mapItem == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanViewTerritory(mapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    filter.PrepareFilter();
                    filter.MapItemId = mapItem.Id;
                    int pageIndex = page ?? 0;
                    var itemList = CheckInContext.GetList(dataContext, filter, pageIndex);
                    var itemListModel = new CheckInListModel(itemList, pageIndex, language);
                    var model = new CheckInPageModel(itemListModel, filter, mapItem);
                    model.CanUpdate = user.CanUpdateTerritory(mapItem.Territory);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: CheckIn/Create
        public ActionResult Create(int mapItemId)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItem = MapItemContext.GetDetail(dataContext, mapItemId);
                    if (mapItem == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanViewTerritory(mapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new CheckInUpdateModel()
                    {
                        IsCreate = true,
                        MapItemId = mapItemId,
                        CheckTime = ContextUtils.FormatDateTime(DateTime.Now, language, true),
                    };
                    model.FillCodeLists(dataContext, mapItem);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CheckIn/Create
        [HttpPost]
        public ActionResult Create(CheckInUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var mapItem = MapItemContext.GetDetail(dataContext, model.MapItemId);
                    if (mapItem == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanViewTerritory(mapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = CheckInContext.Update(dataContext, null, model, user, language);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, CheckInRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "CheckIn", new { id = model.MapItemId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, CheckInRes.ERROR_CREATE);
                        }
                    }
                    model.FillCodeLists(dataContext, mapItem);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: CheckIn/Update/id
        public ActionResult Update(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var checkIn = CheckInContext.GetDetail(dataContext, id);
                    if (checkIn == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(checkIn.MapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "CheckIn", new { id = checkIn.MapItemId });
                    }
                    var model = new CheckInUpdateModel(checkIn, language);
                    model.FillCodeLists(dataContext, checkIn.MapItem);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CheckIn/Update
        [HttpPost]
        public ActionResult Update(CheckInUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var checkIn = CheckInContext.GetDetail(dataContext, model.Id);
                    if (checkIn == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(checkIn.MapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "CheckIn", new { id = checkIn.MapItemId });
                    }

                    CheckInContext.Validate(dataContext, model, ModelState);
                    if (ModelState.IsValid)
                    {
                        var newItemId = CheckInContext.Update(dataContext, checkIn, model, user, language);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, CheckInRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "CheckIn", new { id = model.MapItemId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, CheckInRes.ERROR_UPDATE);
                        }
                    }
                    model.FillCodeLists(dataContext, checkIn.MapItem);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CheckIn/Delete
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
                    var checkIn = CheckInContext.GetDetail(dataContext, id);
                    if (checkIn == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(checkIn.MapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "CheckIn", new { id = checkIn.MapItemId });
                    }

                    var isSuccess = CheckInContext.Delete(dataContext, checkIn, user);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, CheckInRes.SUCCESS_DELETE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, CheckInRes.ERROR_DELETE);
                    }
                    return RedirectToAction("Index", "CheckIn", new { id = checkIn.MapItemId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CheckIn/SelectQuestionnaireAjax/id
        [HttpPost]
        public ActionResult SelectQuestionnaireAjax(CheckInUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_ALLOWED);
                    }
                    ModelState.Clear();
                    if (model.QuestionnaireId.HasValue)
                    {
                        var questionnaire = QuestionnaireContext.GetDetail(dataContext, model.QuestionnaireId.Value);
                        if (questionnaire == null)
                        {
                            return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_FOUND);
                        }
                        if (user.CanViewTerritory(questionnaire.MapItemType.Territory) == false)
                        {
                            return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_ALLOWED);
                        }
                        model.FillQuestionnaire(null, questionnaire, language);
                        model.FillCodeLists(dataContext, null);
                    }
                    return PartialView(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                return PartialView(@"AjaxError", GlobalRes.ERROR_EXCEPTION);
            }
        }

        // GET: CheckIn/Detail/id
        public ActionResult Detail(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var language = LanguageContext.GetLanguage(languageId);
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var checkIn = CheckInContext.GetDetail(dataContext, id);
                    if (checkIn == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanViewTerritory(checkIn.MapItem.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new CheckInDetailModel(checkIn, language);
                    model.CanUpdate = user.CanUpdateTerritory(checkIn.MapItem.Territory);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CheckInController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

    }
}