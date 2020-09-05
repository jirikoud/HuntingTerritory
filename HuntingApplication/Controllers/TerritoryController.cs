using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuntingModel.Context;
using HuntingModel.Localization;
using HuntingModel.Model;
using HuntingModel.ViewModel;

namespace HuntingApplication.Controllers
{
    [Authorize]
    public class TerritoryController : Controller
    {
        // GET: Territory
        public ActionResult Index()
        {
            return View();
        }

        // GET: Territory/Detail/id
        public ActionResult Detail(int id)
        {
            using (var dataContext = new HuntingEntities())
            {
                var territory = TerritoryContext.GetTerritory(dataContext, id);
                var model = new TerritoryDetailViewModel(territory);
                return View(model);
            }
        }

        // GET: Territory/Show/id
        [AllowAnonymous]
        public ActionResult Show(int id)
        {
            using (var dataContext = new HuntingEntities())
            {
                var territory = TerritoryContext.GetTerritory(dataContext, id);
                var model = new TerritoryDetailViewModel(territory);
                return View(model);
            }
        }

        // GET: Territory/EditMap/id
        public ActionResult EditMap(int id)
        {
            using (var dataContext = new HuntingEntities())
            {
                var territory = TerritoryContext.GetTerritory(dataContext, id);
                var model = new TerritoryDetailViewModel(territory);
                return View(model);
            }
        }

        // POST: Territory/SaveMapAjax
        [HttpPost]
        public ActionResult SaveMapAjax(int id, string polygon)
        {
            using (var dataContext = new HuntingEntities())
            {
                var saveModel = TerritoryContext.ParseMapSaveString(polygon);
                var model = TerritoryContext.SaveMap(dataContext, id, saveModel);
                return View(model);
            }
        }

        // GET: Territory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Territory/Create
        [HttpPost]
        public ActionResult Create(TerritoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var dataContext = new HuntingEntities())
            {
                var user = HomeController.GetLoggedUser(dataContext);
                var newItem = TerritoryContext.Create(dataContext, model, user);
                if (newItem != null)
                {
                    return RedirectToAction("Detail", new { id = newItem.Id });
                }
                ModelState.AddModelError(string.Empty, TerritoryRes.ERROR_CREATE);
            }
            return View(model);
        }

        // GET: Territory/List
        [AllowAnonymous]
        public ActionResult List()
        {
            return View();
        }

        // GET: Territory/ListAjax
        [AllowAnonymous]
        public ActionResult ListAjax(int pageIndex, string searchString)
        {
            TerritoryListViewModel model = null;
            using (var dataContext = new HuntingEntities())
            {
                var pageCount = TerritoryContext.GetPageCount(dataContext, pageIndex, searchString);
                var itemList = TerritoryContext.GetList(dataContext, pageIndex, searchString);
                model = new TerritoryListViewModel(itemList, searchString, pageIndex, pageCount);

            }
            return View(model);
        }
    }
}