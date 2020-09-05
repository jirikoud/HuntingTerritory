using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuntingModel.Context;
using HuntingModel.Model;
using HuntingModel.ViewModel;
using Microsoft.AspNet.Identity;
using NLog;

namespace HuntingApplication.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static HunterUser GetLoggedUser(HuntingEntities dataContext)
        {
            logger.Warn("GetLoggedUser IsAuthenticated={0}", System.Web.HttpContext.Current.User.Identity.IsAuthenticated);
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                logger.Warn("GetLoggedUser GetUserId={0}", System.Web.HttpContext.Current.User.Identity.GetUserId());
                var user = HunterUserContext.GetUserByName(dataContext, System.Web.HttpContext.Current.User.Identity.GetUserId());
                return user;
            }
            return null;
        }

        public ActionResult Index()
        {
            HomeViewModel model;
            using (var dataContext = new HuntingEntities())
            {
                var user = GetLoggedUser(dataContext);
                if (user != null)
                {
                    model = new HomeViewModel(user);
                }
                else
                {
                    model = new HomeViewModel();
                }
            }
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}