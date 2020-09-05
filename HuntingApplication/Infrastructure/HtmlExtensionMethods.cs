using HuntingModel.Localization;
using HuntingModel.SqlGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;

namespace System.Web.Mvc.Html
{
    public static class HtmlExtensionMethods
    {
        public static MvcHtmlString CustomValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            var htmlString = htmlHelper.ValidationSummary(excludePropertyErrors, "", new { @class = "text-danger" });

            if (htmlString != null)
            {
                XElement xEl = XElement.Parse(htmlString.ToHtmlString());

                var lis = xEl.Element("ul").Elements("li");

                if (lis.Count() == 1 && lis.First().Value == "")
                    return null;
            }

            return htmlString;
        }

        public static IHtmlString LanguageLink(this HtmlHelper htmlHelper, string languageCode, string languageName)
        {
            var controllerName = htmlHelper.ViewContext.ParentActionViewContext.RouteData.Values["controller"].ToString();
            var actionName = htmlHelper.ViewContext.ParentActionViewContext.RouteData.Values["action"].ToString();
            var routeData = new RouteValueDictionary();
            foreach (var key in htmlHelper.ViewContext.ParentActionViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                routeData.Add(key, htmlHelper.ViewContext.ParentActionViewContext.HttpContext.Request.QueryString[key]);
            }
            foreach (var value in htmlHelper.ViewContext.ParentActionViewContext.RouteData.Values)
            {
                if (value.Key == "id")
                {
                    routeData.Add("id", value.Value);
                }
            }
            routeData["lang"] = languageCode;
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("title", languageName);
            htmlAttributes.Add("data-toggle", "tooltip");
            htmlAttributes.Add("data-placement", "top");
            return htmlHelper.ActionLink(languageName, actionName, controllerName, routeData, htmlAttributes);
        }

        public static IHtmlString PagerLink(this HtmlHelper htmlHelper, string linkText, string actionName, int pageIndex, bool isSelected = false)
        {
            if (isSelected)
            {
                return MvcHtmlString.Create("<li class=\"active\"><span>" + linkText + "</span></li>");
            }
            var routeData = new RouteValueDictionary();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                routeData.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
            }
            routeData["page"] = pageIndex;
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("title", linkText);
            return MvcHtmlString.Create("<li>" + htmlHelper.ActionLink(linkText, actionName, routeData, htmlAttributes).ToString() + "</li>");
        }

        public static IHtmlString SortingLink(this HtmlHelper htmlHelper, string field, string actionName, bool isSortDown, FilterBase filter)
        {
            bool isSelected = (filter.SortField == field && filter.SortIsDesc == isSortDown);
            var classString = "glyphicon tinyContr " + (isSortDown ? "glyphicon-chevron-down" : "glyphicon-chevron-up") + (isSelected ? " active" : null);

            var routeData = new RouteValueDictionary();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                routeData.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
            }
            routeData["filter.sortfield"] = field;
            routeData["filter.sortisdesc"] = isSortDown;
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("title", isSortDown ? GlobalRes.TITLE_SORT_DESC : GlobalRes.TITLE_SORT_ASC);
            htmlAttributes.Add("class", classString);
            htmlAttributes.Add("data-toggle", "tooltip");
            htmlAttributes.Add("data-placement", "top");
            return htmlHelper.ActionLink(" ", actionName, routeData, htmlAttributes);
        }

        public static IHtmlString FilterLink(this HtmlHelper htmlHelper, string actionName, string filterField, string filterValue, string title)
        {
            var routeData = new RouteValueDictionary();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                routeData.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
            }
            routeData["filter." + filterField] = filterValue;
            routeData["page"] = 0;
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("title", title);
            return htmlHelper.ActionLink(title, actionName, routeData, htmlAttributes);
        }

        public static IHtmlString ResetFilterLink(this HtmlHelper htmlHelper, string actionName, FilterBase filter)
        {
            var routeData = new RouteValueDictionary();
            foreach (var key in htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                if (key == "filter.Panel")
                {
                    routeData.Add(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
                }
            }
            routeData["filter.sortfield"] = filter.SortField;
            routeData["filter.sortisdesc"] = filter.SortIsDesc;
            var htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("title", GlobalRes.BUTTON_RESET_FILTER);
            htmlAttributes.Add("class", "filter reset");
            htmlAttributes.Add("data-toggle", "tooltip");
            htmlAttributes.Add("data-placement", "top");
            var repID = Guid.NewGuid().ToString();
            var contentString = "<span class=\"glyphicon glyphicon-remove\"></span>" + GlobalRes.BUTTON_RESET_FILTER;
            var linkString = htmlHelper.ActionLink(repID, actionName, routeData, htmlAttributes).ToString();
            return MvcHtmlString.Create(linkString.Replace(repID, contentString));
        }

    }
}