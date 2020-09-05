using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.ViewModel.QuestionModels;
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
    public class QuestionController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Question
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

                    var itemList = QuestionContext.GetList(dataContext, questionnaire);
                    var model = new QuestionListModel(questionnaire, itemList);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Question/Create
        public ActionResult Create(int questionnaireId)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, questionnaireId);
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
                    var model = new QuestionUpdateModel()
                    {
                        IsCreate = true,
                        QuestionnaireId = questionnaireId,
                    };
                    model.FillCodeList(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult Create(QuestionUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, model.QuestionnaireId);
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
                        var newItemId = QuestionContext.Update(dataContext, null, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "Question", new { id = model.QuestionnaireId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionRes.ERROR_CREATE);
                        }
                    }
                    model.FillCodeList(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Question/Update/id
        public ActionResult Update(int id)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var question = QuestionContext.GetDetail(dataContext, id);
                    if (question == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(question.Questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = question.Questionnaire.MapItemType.TerritoryId });
                    }
                    var model = new QuestionUpdateModel(question);
                    model.FillCodeList(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Question/Update
        [HttpPost]
        public ActionResult Update(QuestionUpdateModel model)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var question = QuestionContext.GetDetail(dataContext, model.Id);
                    if (question == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(question.Questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = question.Questionnaire.MapItemType.TerritoryId });
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = QuestionContext.Update(dataContext, question, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "Question", new { id = model.QuestionnaireId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionRes.ERROR_UPDATE);
                        }
                    }
                    model.FillCodeList(languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
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
                    var question = QuestionContext.GetDetail(dataContext, id);
                    if (question == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(question.Questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = question.Questionnaire.MapItemType.TerritoryId });
                    }

                    var isSuccess = QuestionContext.Delete(dataContext, question, user);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, QuestionRes.SUCCESS_DELETE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionRes.ERROR_DELETE);
                    }
                    return RedirectToAction("Index", "Question", new { id = question.QuestionnaireId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questionnaire/Delete
        [HttpPost]
        public ActionResult Move(int id, bool isMoveUp)
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
                    var question = QuestionContext.GetDetail(dataContext, id);
                    if (question == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(question.Questionnaire.MapItemType.Territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = question.Questionnaire.MapItemType.TerritoryId });
                    }

                    var isSuccess = QuestionContext.Move(dataContext, question.Questionnaire, question.Id, isMoveUp, user);
                    if (isSuccess == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, QuestionRes.ERROR_MOVE);
                    }
                    return RedirectToAction("Index", "Question", new { id = question.QuestionnaireId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Questionnaire/OptionTableAjax
        [HttpPost]
        public ActionResult OptionTableAjax(QuestionUpdateModel model, string formAction, int? index)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_ALLOWED);
                    }
                    var questionnaire = QuestionnaireContext.GetDetail(dataContext, model.QuestionnaireId);
                    if (questionnaire == null)
                    {
                        return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_FOUND);
                    }
                    if (user.CanUpdateTerritory(questionnaire.MapItemType.Territory) == false)
                    {
                        return PartialView(@"AjaxError", GlobalRes.ERROR_NOT_ALLOWED);
                    }

                    QuestionContext.OptionTableAction(dataContext, model, formAction, index);
                    ModelState.Clear();
                    return PartialView("~/Views/Question/CommonLayout/OptionList.cshtml", model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "QuestionController");
                return PartialView(@"AjaxError", GlobalRes.ERROR_EXCEPTION);
            }
        }
    }
}