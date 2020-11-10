using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web;
using HuntingCoreModel.Database;

namespace HuntingCoreModel.Infrastructure
{
    public static class ContextUtils
    {
        //public static string FullyQualifiedApplicationPath
        //{
        //    get
        //    {
        //        //Return variable declaration
        //        string appPath = null;

        //        //Getting the current context of HTTP request
        //        HttpContext context = HttpContext.Current;

        //        //Checking the current context content
        //        if (context != null)
        //        {
        //            //Formatting the fully qualified website url/name
        //            appPath = string.Format("{0}://{1}{2}{3}",
        //              context.Request.Url.Scheme,
        //              context.Request.Url.Host,
        //              context.Request.Url.Port == 80
        //                ? string.Empty : ":" + context.Request.Url.Port,
        //              context.Request.ApplicationPath);
        //        }
        //        if (!appPath.EndsWith("/"))
        //            appPath += "/";
        //        return appPath;
        //    }
        //}

        public static int NormalizedPageSize(int pageSize)
        {
            if (pageSize > Constants.MAX_LIST_PAGE_SIZE)
            {
                return Constants.MAX_LIST_PAGE_SIZE;
            }
            if (pageSize < 1)
            {
                return Constants.DEFAULT_LIST_PAGE_SIZE;
            }
            return pageSize;
        }

        public static string GetFulltextString(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return null;
            }
            filter = filter.Replace("\"", string.Empty);
            var wordList = filter.Split();
            var fulltext = string.Join(" OR ", wordList.Select(item => string.Format("\"{0}*\"", item)));
            return fulltext;
        }

        //public static ListPaging GetPaging(int pageIndex, int pageSize, bool? takeAll = false)
        //{
        //    var paging = new ListPaging(NormalizedPageSize(pageSize), pageIndex, takeAll);
        //    return paging;
        //}

        public static List<int> ProcessDocumentIdList(string listString)
        {
            if (string.IsNullOrWhiteSpace(listString))
            {
                return new List<int>();
            }
            var list = new List<int>(listString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Select(item => item.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Last()).Select(int.Parse).Distinct());
            return list;
        }

        public static List<int> ProcessIdList(string listString)
        {
            if (string.IsNullOrWhiteSpace(listString))
            {
                return new List<int>();
            }
            var list = new List<int>();
            var textArray = listString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var textValue in textArray)
            {
                int intValue;
                if (int.TryParse(textValue, out intValue))
                {
                    list.Add(intValue);
                }
            }
            return list;
        }

        public static string JoinIntList(IEnumerable<int> intList)
        {
            if (intList != null)
            {
                return string.Join("|", intList);
            }
            return null;
        }

        public static string JoinStringList(string[] stringList)
        {
            if (stringList != null)
            {
                return string.Join("|", stringList);
            }
            return null;
        }

        public static string TrimString(string value)
        {
            if (value == null)
            {
                return null;
            }
            return value.Trim();
        }

        public static string CreateLikeParam(string searchWord)
        {
            if (!searchWord.EndsWith("*", StringComparison.Ordinal) && !searchWord.Contains('*'))
            {
                searchWord += "*";
            }
            searchWord = searchWord.Replace("%", "[%]");
            searchWord = searchWord.Replace('*', '%');
            return searchWord;
        }

        public static string CreateContainsParam(string searchPhrase)
        {
            return GetFulltextString(searchPhrase);
        }

        public static double? GetCurrencyValue(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }
            stringValue = stringValue.Replace(" ", string.Empty).Replace(".", ",");
            double value;
            if (double.TryParse(stringValue, NumberStyles.Currency, CultureInfo.GetCultureInfo("cs-CZ"), out value))
            {
                return value;
            }
            return null;
        }

        public static string GetSortClass(bool isDescending)
        {
            if (isDescending == false)
            {
                return "sort-active sort-asc";
            }
            return "sort-active sort-desc";
        }

        //public static void CreateActionStateCookie(HttpResponseBase response, ActionTypeEnum actionType, string message)
        //{
        //    var cookie = new HttpCookie(Constants.ACTION_STATE_COOKIE);
        //    cookie.Values.Add(Constants.COOKIE_VALUE_ACTION_TYPE, actionType.ToString());
        //    var encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        //    cookie.Values.Add(Constants.COOKIE_VALUE_MESSAGE, encodedMessage);
        //    response.Cookies.Add(cookie);
        //}

        //public static ActionModel ReadActionStateCookie(HttpRequestBase request, HttpResponseBase response)
        //{
        //    if (request.Cookies.AllKeys.Contains(Constants.ACTION_STATE_COOKIE))
        //    {
        //        var cookie = request.Cookies[Constants.ACTION_STATE_COOKIE];
        //        cookie.Expires = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //        response.Cookies.Add(cookie);
        //        ActionTypeEnum actionType;
        //        if (Enum.TryParse(cookie.Values[Constants.COOKIE_VALUE_ACTION_TYPE], out actionType))
        //        {
        //            var message = Encoding.UTF8.GetString(Convert.FromBase64String(cookie.Values[Constants.COOKIE_VALUE_MESSAGE]));
        //            return new ActionModel() { ActionType = actionType, ActionMessage = message };
        //        }
        //    }
        //    return null;
        //}

        public static DateTime? ParseDateTimeString(string dateString, Language language)
        {
            var format = language != null ? string.Format("{0} {1}", language.DateFormat, language.TimeFormat) : "N/A";
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }
            try
            {
                var result = DateTime.ParseExact(dateString, format, CultureInfo.CurrentUICulture);
                return result;
            }
            catch (Exception exception)
            {
                //logger.Info(exception, "ParseDateTimeString {0}, format {1}", dateString, format);
                return null;
            }
        }

        public static DateTime? ParseDateString(string dateString, Language language)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }
            try
            {
                var result = DateTime.ParseExact(dateString, language.DateFormat, CultureInfo.CurrentUICulture);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static long? ParseTimeStringToTicks(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                return null;
            }
            try
            {
                var timeSpan = TimeSpan.Parse(timeString, CultureInfo.CurrentUICulture);
                return timeSpan.Ticks;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static double? ParseFloatString(string floatString)
        {
            double result = 0;
            if (double.TryParse(floatString, NumberStyles.Any, CultureInfo.CurrentUICulture, out result))
            {
                return result;
            }
            if (double.TryParse(floatString, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            return null;
        }

        public static string FormatDateTime(DateTime? dateTime, Language language, bool isForceTime = false)
        {
            if (dateTime == null)
            {
                return null;
            }
            if (dateTime.Value.TimeOfDay.Ticks > 0 || isForceTime)
            {
                var result = string.Format("{0} {1}", 
                    dateTime.Value.ToString(language.DateFormat), 
                    dateTime.Value.ToString(language.TimeFormat));
                return result;
            }
            var dateResult = dateTime.Value.ToString(language.DateFormat);
            return dateResult;
        }

        public static string FormatFloat(double? doubleValue, Language language)
        {
            if (doubleValue == null)
            {
                return null;
            }
            var doubleResult = doubleValue.Value.ToString(CultureInfo.CurrentUICulture);
            return doubleResult;
        }

        public static long DateTimeToUnixTime(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
        }

        public static DateTime DateTimeFromUnixTime(long unixTime)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            return offset.DateTime;
        }
    }
}
