using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuntingModel.Context;
using HuntingModel.Localization;
using HuntingModel.ViewModel;
using HuntingApplication.Infrastructure;
using NLog;
using HuntingModel.Database;
using HuntingModel.Infrastructure;
using HuntingModel.Enumeration;
using HuntingModel.ViewModel.TerritoryModels;
using Newtonsoft.Json;
using HuntingModel.SqlGenerator.Generators;

namespace HuntingApplication.Controllers
{
    [Authorize]
    [Localization]
    [AllowGoogleMap]
    public class TerritoryController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Territory/id
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
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanViewTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    var model = new TerritoryDetailModel(territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        //GET: Territory/List
        public ActionResult List(TerritoryFilter filter, int? page)
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
                    if (user.AccountTypeEx == AccountTypeEnum.Demo)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    filter.PrepareFilter(languageId);
                    filter.IsContact = true;
                    int pageIndex = page ?? 0;
                    var itemList = TerritoryContext.GetList(dataContext, filter, pageIndex, user.Id);
                    var itemListModel = new TerritoryListModel(itemList, pageIndex);
                    var model = new TerritoryPageModel(itemListModel, filter);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                return RedirectToAction("Index", "Home");
            }
        }

        //POST: Territory/Contact
        public ActionResult Contact(int id, string message)
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
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (territory == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("List", "Territory");
                    }
                    if (territory.StewardId == user.Id)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("List", "Territory");
                    }
                    if (territory.TerritoryUsers.Any(item => item.AclUserId == user.Id))
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("List", "Territory");
                    }
                    if (territory.TerritoryUserContacts.Any(item => item.IsDeleted == false && item.AclUserId == user.Id))
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("List", "Territory");
                    }

                    var result = TerritoryContext.Contact(dataContext, territory, user, message);
                    if (result)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, TerritoryRes.SUCCESS_CONTACT);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, TerritoryRes.ERROR_CONTACT);
                    }
                    return RedirectToAction("List", "Territory");
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                return RedirectToAction("List", "Territory");
            }
        }

        // GET: Territory/Create
        public ActionResult Create()
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
                    if (user.CanCreateTerritory() == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new TerritoryUpdateModel()
                    {
                        IsCreate = true,
                    };
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Territory/Create
        [HttpPost]
        public ActionResult Create(TerritoryUpdateModel model)
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
                    if (user.CanCreateTerritory() == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = TerritoryContext.Update(dataContext, null, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, TerritoryRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "Territory", new { id = newItemId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, TerritoryRes.ERROR_CREATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Territory/Update/id
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
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = id });
                    }
                    var model = new TerritoryUpdateModel(territory);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Territory", new { id = id });
            }
        }

        // POST: Territory/Update
        [HttpPost]
        public ActionResult Update(TerritoryUpdateModel model)
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
                    var territory = TerritoryContext.GetDetail(dataContext, model.Id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = model.Id });
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = TerritoryContext.Update(dataContext, territory, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, TerritoryRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "Territory", new { id = model.Id });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, TerritoryRes.ERROR_UPDATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Territory/UpdateMap
        public ActionResult UpdateMap(int id)
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
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Home");
                    }
                    var model = new TerritoryDetailModel(territory, user);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Territory/SaveMapAjax
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveMapAjax(int id, string mapData)
        {
            var model = new TerritoryMapSavedModel();
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return PartialView(model);
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return PartialView(model);
                    }

                    var saveModel = JsonConvert.DeserializeObject<MapSaveDataModel>(mapData);
                    model = TerritoryContext.SaveMap(dataContext, id, saveModel, user.Id);
                    return PartialView(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return PartialView(model);
            }
        }

        // GET: Territory/PersonList/id
        public ActionResult PersonList(int id)
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
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = id });
                    }
                    int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
                    var model = new TerritoryPersonListModel(territory, languageId);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Territory", new { id = id });
            }
        }

        // POST: Territory/PersonList/
        [HttpPost]
        public ActionResult PersonList(TerritoryPersonListModel model)
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
                    var territory = TerritoryContext.GetDetail(dataContext, model.Id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = model.Id });
                    }
                    var result = TerritoryContext.UpdateUserList(dataContext, territory, model);
                    if (result == true)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, TerritoryRes.SUCCESS_UPDATE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, TerritoryRes.ERROR_UPDATE);
                    }
                    return RedirectToAction("PersonList", "Territory", new { id = model.Id });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Territory/InviteUser
        [HttpPost]
        public ActionResult InviteUser(int id, string email, string name)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        return Content("ERROR");
                    }
                    var territory = TerritoryContext.GetDetail(dataContext, id);
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        return Content("ERROR");
                    }

                    var result = TerritoryContext.InviteUser(dataContext, territory, email, name, user);
                    if (result == true)
                    {
                        return Content("OK");
                    }
                    return Content("CREATE");
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                return Content("ERROR");
            }
        }

        // POST: Territory/ContactAction
        [HttpPost]
        public ActionResult ContactAction(int id, bool accept)
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    var user = AclUserContext.GetDetail(dataContext, User.Identity.Name);
                    if (user == null)
                    {
                        return Content("NOT_ALLOWED");
                    }
                    var contact = TerritoryContext.GetTerritoryUserContact(dataContext, id);
                    if (contact == null)
                    {
                        return Content("NOT_FOUND");
                    }
                    if (contact.Territory.StewardId != user.Id)
                    {
                        return Content("NOT_ALLOWED");
                    }

                    var result = TerritoryContext.ContactAction(dataContext, contact, accept, user);
                    if (result == true)
                    {
                        return Content("OK");
                    }
                    return Content("ERROR");
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                return Content("ERROR");
            }
        }

        // POST: MapItemType/Delete
        [HttpPost]
        public ActionResult DeleteUser(int territoryId, int id)
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
                    var territory = TerritoryContext.GetDetail(dataContext, territoryId);
                    if (territory == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = territoryId });
                    }

                    var isSuccess = TerritoryContext.DeleteUser(dataContext, territory, id, user);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, TerritoryRes.SUCCESS_DELETE_USER);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, TerritoryRes.ERROR_DELETE_USER);
                    }
                    return RedirectToAction("PersonList", "Territory", new { id = territoryId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "TerritoryController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}