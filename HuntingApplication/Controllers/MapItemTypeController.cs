using HuntingApplication.Infrastructure;
using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.ViewModel.MapItemTypeModels;
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
    public class MapItemTypeController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: MapItemType/id
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
                    if (territory == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = id });
                    }

                    var itemList = MapItemTypeContext.GetList(dataContext, territory);
                    var model = new MapItemTypeListModel(territory, itemList);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Territory", new { id = id });
            }
        }

        // GET: MapItemType/Create
        public ActionResult Create(int territoryId)
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
                    var model = new MapItemTypeUpdateModel()
                    {
                        IsCreate = true,
                        TerritoryId = territoryId,
                    };
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Territory", new { id = territoryId });
            }
        }

        // POST: MapItemType/Create
        [HttpPost]
        public ActionResult Create(MapItemTypeUpdateModel model)
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
                    var territory = TerritoryContext.GetDetail(dataContext, model.TerritoryId);
                    if (territory == null)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_FOUND);
                        return RedirectToAction("Index", "Home");
                    }
                    if (user.CanUpdateTerritory(territory) == false)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, GlobalRes.ERROR_NOT_ALLOWED);
                        return RedirectToAction("Index", "Territory", new { id = model.TerritoryId });
                    }

                    if (ModelState.IsValid)
                    {
                        var newItemId = MapItemTypeContext.Update(dataContext, null, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, MapItemTypeRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", "MapItemType", new { id = model.TerritoryId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, MapItemTypeRes.ERROR_CREATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: MapItemType/Update/id
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
                    var model = new MapItemTypeUpdateModel(mapItemType);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: MapItemType/Update
        [HttpPost]
        public ActionResult Update(MapItemTypeUpdateModel model)
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
                    var mapItemType = MapItemTypeContext.GetDetail(dataContext, model.Id);
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
                        var newItemId = MapItemTypeContext.Update(dataContext, mapItemType, model, user);
                        if (newItemId.HasValue)
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, MapItemTypeRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", "MapItemType", new { id = model.TerritoryId });
                        }
                        else
                        {
                            ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, MapItemTypeRes.ERROR_UPDATE);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: MapItemType/Delete
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

                    var isSuccess = MapItemTypeContext.Delete(dataContext, mapItemType, user);
                    if (isSuccess)
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Success, MapItemTypeRes.SUCCESS_DELETE);
                    }
                    else
                    {
                        ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Warning, MapItemTypeRes.ERROR_DELETE);
                    }
                    return RedirectToAction("Index", "MapItemType", new { id = mapItemType.TerritoryId });
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "MapItemTypeController");
                ContextUtils.CreateActionStateCookie(Response, ActionTypeEnum.Danger, GlobalRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}