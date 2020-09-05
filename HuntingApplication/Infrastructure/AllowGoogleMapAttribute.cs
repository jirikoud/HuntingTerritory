using HuntingModel.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuntingApplication.Infrastructure
{
    public class AllowGoogleMapAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.GoogleMapKey = Settings.Default.GoogleMapsAPIKey;
        }
    }
}