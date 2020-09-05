using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Cache
{
    public static class LocaleCache
    {
        private static object lockObject = new object();

        private static Dictionary<string, CultureInfo> cacheLocale = new Dictionary<string, CultureInfo>();

        public static CultureInfo CheckInCache(string locale)
        {
            lock (lockObject)
            {
                if (cacheLocale.ContainsKey(locale))
                {
                    return cacheLocale[locale];
                }
            }
            return null;
        }

        public static void AddToCache(string locale, CultureInfo cultureInfo)
        {
            lock (lockObject)
            {
                cacheLocale[locale] = cultureInfo;
            }
        }
    }
}
