using HuntingModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HuntingModel.Infrastructure
{
    public class FilterUtils
    {
        private static bool GetSortDescending(string sortField)
        {
            if (sortField == "desc")
            {
                return true;
            }
            return false;
        }

        public static bool IsFulltextColumn(HttpRequestBase request, string columnName, string columnAll)
        {
            if (request.QueryString[columnName] == "1")
            {
                return true;
            }
            if (request.QueryString[columnAll] == "1")
            {
                return true;
            }
            return false;
        }

        public static int? FilterIntValue(HttpRequestBase request, string columnName, int? defaultValue = null)
        {
            int intValue;
            if (request.QueryString[columnName] != null && Int32.TryParse(request.QueryString[columnName], out intValue))
            {
                return intValue;
            }
            return defaultValue;
        }

        public static DateTime? FilterDateTimeValue(HttpRequestBase request, string columnName)
        {
            DateTime dateTimeValue;
            if (request.QueryString[columnName] != null && DateTime.TryParse(request.QueryString[columnName], out dateTimeValue))
            {
                return dateTimeValue;
            }
            return null;
        }

        public static bool? FilterBoolValue(HttpRequestBase request, string columnName)
        {
            bool boolValue;
            if (request.QueryString[columnName] != null && bool.TryParse(request.QueryString[columnName], out boolValue))
            {
                return boolValue;
            }
            return null;
        }

        public static string FilterStringValue(HttpRequestBase request, string columnName)
        {
            var stringValue = request.QueryString[columnName];
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                return stringValue;
            }
            return null;
        }

        public static int[] FilterIntArrayValue(HttpRequestBase request, string columnName)
        {
            var queryValue = request.QueryString[columnName];
            if (queryValue != null && queryValue != "null")
            {
                var intArray = ContextUtils.ProcessIdList(request.QueryString[columnName]).ToArray();
                return intArray;
            }
            return null;
        }

        public static string[] FilterStringArrayValue(HttpRequestBase request, string columnName)
        {
            var queryValue = request.QueryString[columnName];
            if (queryValue != null && queryValue != "null")
            {
                var stringArray = request.QueryString[columnName].Split(',');
                return stringArray;
            }
            return null;
        }

        public static T? FilterEnumValue<T>(HttpRequestBase request, string columnName) where T : struct
        {
            var queryValue = request.QueryString[columnName];
            if (queryValue != null && queryValue != "null")
            {
                T result;
                if (Enum.TryParse<T>(queryValue, out result))
                {
                    return result;
                }
            }
            return null;
        }

        public static PagerModel GetPager(int pageCount, int currentPage)
        {
            if (pageCount == 0)
            {
                return null;
            }
            var pager = new PagerModel();
            int maxIndex = pageCount - 1;
            if (currentPage > 0)
            {
                pager.FirstPage = new PageModel(0);
                pager.PrevPage = new PageModel(currentPage - 1);
            }
            if (currentPage < maxIndex)
            {
                pager.LastPage = new PageModel(pageCount - 1);
                pager.NextPage = new PageModel(currentPage + 1);
            }

            pager.CenterPager = new List<PageModel>();
            int centerMin = currentPage - (Constants.PAGER_LINK_COUNT / 2);
            int centerMax = currentPage + (Constants.PAGER_LINK_COUNT / 2);
            if (centerMin < 0)
            {
                centerMax -= centerMin;
                centerMin = 0;
            }
            if (centerMax > maxIndex)
            {
                centerMin -= centerMax - maxIndex;
                if (centerMin < 0)
                {
                    centerMin = 0;
                }
                centerMax = maxIndex;
            }
            for (int pageIndex = centerMin; pageIndex <= centerMax; pageIndex++)
            {
                pager.CenterPager.Add(new PageModel(pageIndex, pageIndex == currentPage));
            }

            return pager;
        }

        public static bool FilterMultiEditValue(HttpRequestBase request)
        {
            var queryValue = request.QueryString["editMode"];
            if (queryValue == "on")
            {
                return true;
            }
            return false;
        }

        public static string GetFilterString(HttpRequestBase request)
        {
            var keyList = request.QueryString.AllKeys.Where(item =>
                item != null && (item.ToLowerInvariant().StartsWith("fulltext") || item.ToLowerInvariant().StartsWith("filter") || item.ToLowerInvariant().StartsWith("whereList")));
            var filterString = string.Join("&", keyList.Select(item => string.Format("{0}={1}", item, request.QueryString[item])));
            return filterString;
        }
    }
}
