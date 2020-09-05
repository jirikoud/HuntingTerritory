using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using NLog;
using HuntingModel.Context;
using HuntingModel.Cache;
using HuntingModel.Database;

namespace HuntingApplication.Infrastructure
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public const string SESSION_LANGUAGE_ID = "SESSION_LANGUAGE_ID";
        public const string COOKIE_LANGUAGE = "CookieLanguage";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private void SetLocale(ActionExecutingContext filterContext, string code, bool fromRoute)
        {
            var cultureInfo = LocaleCache.CheckInCache(code);
            if (cultureInfo != null)
            {
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            else
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(code);
                }
                catch (Exception)
                {
                    logger.Warn("Localization - \"{0}\" is not a valid locale in {1}", code, fromRoute ? "route" : "header");
                    code = Thread.CurrentThread.CurrentUICulture.Name;
                }
                LocaleCache.AddToCache(code, Thread.CurrentThread.CurrentUICulture);
            }
            filterContext.RouteData.Values["lang"] = Thread.CurrentThread.CurrentUICulture.Name;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string code = null;
            Language language = null;
            if (filterContext.RouteData.Values["lang"] != null &&
                string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()) == false)
            {
                // set the culture from the route data (url)
                code = filterContext.RouteData.Values["lang"].ToString();
                language = LanguageContext.GetLanguage(code);
                SetLocale(filterContext, language.Code, true);
            }
            else
            {
                // load the culture info from the cookie
                var cookie = filterContext.HttpContext.Request.Cookies[COOKIE_LANGUAGE];
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    code = cookie.Value;
                }
                else
                {
                    var languages = filterContext.HttpContext.Request.UserLanguages;
                    if (languages != null && languages.Length > 0)
                    {
                        // set the culture by the location if not specified
                        code = filterContext.HttpContext.Request.UserLanguages[0];
                    }
                }
                language = LanguageContext.GetLanguage(code);
                SetLocale(filterContext, language.Code, false);
            }
            HttpContext.Current.Session[SESSION_LANGUAGE_ID] = language.Id;
            filterContext.Controller.ViewBag.LangShort = language.Shortcut;

            // save the location into cookie
            HttpCookie saveCookie = new HttpCookie(COOKIE_LANGUAGE, Thread.CurrentThread.CurrentUICulture.Name);
            saveCookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(saveCookie);

            base.OnActionExecuting(filterContext);
        }
    }
}
