using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuntingModel.Context;
using NLog;
using HuntingApplication.Infrastructure;
using HuntingModel.ViewModel.HomeModels;
using HuntingModel.Database;
using HuntingModel.Infrastructure;
using HuntingModel.Properties;

namespace HuntingApplication.Controllers
{
    [Localization]
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [ChildActionOnly]
        public ActionResult ScriptDefine()
        {
            int languageId = (int)Session[LocalizationAttribute.SESSION_LANGUAGE_ID];
            var language = LanguageContext.GetLanguage(languageId);
            var model = new ScriptDefineModel()
            {
                IsDebug = true,
                Lang = language.Shortcut,
                Locale = language.Code,
                DateFormat = language.DateFormatJS,
                TimeFormat = language.TimeFormatJS,
                MapAPIKey = Settings.Default.GoogleMapsAPIKey,
            };
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult AlertMessage()
        {
            var model = ContextUtils.ReadActionStateCookie(Request, Response);
            return PartialView(model);
        }

        public ActionResult Index()
        {
            try
            {
                using (var dataContext = new HuntingEntities())
                {
                    if (Request.IsAuthenticated)
                    {
                        var userName = User.Identity.Name;
                        var user = AclUserContext.GetDetail(dataContext, userName);
                        var model = new HomeViewModel(user);
                        return View(model);
                    }
                    return View(new HomeViewModel());
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "HomeController");
                throw;
            }
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