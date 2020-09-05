using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Cache
{
    public class LanguageCache
    {
        private static object lockObject = new object();
        private static List<Language> cacheLanguageList;
        private static Dictionary<string, Language> cacheLanguageCode = new Dictionary<string, Language>();
        private static Dictionary<int, Language> cacheLanguageId = new Dictionary<int, Language>();
        private static Language cacheDefaultLanguage;

        #region LanguageList

        public static List<Language> CheckInCacheLanguageList()
        {
            lock (lockObject)
            {
                if (cacheLanguageList != null)
                {
                    return cacheLanguageList;
                }
            }
            return null;
        }

        public static void AddToCacheLanguageList(List<Language> list)
        {
            lock (lockObject)
            {
                cacheLanguageList = list;
            }
        }

        #endregion

        #region LanguageCode

        public static Language CheckInCacheLanguageCode(string code)
        {
            lock (lockObject)
            {
                if (cacheLanguageCode.ContainsKey(code))
                {
                    return cacheLanguageCode[code];
                }
            }
            return null;
        }

        public static void AddToCacheLanguageCode(string code, Language language)
        {
            lock (lockObject)
            {
                if (cacheLanguageCode.ContainsKey(code) == false)
                {
                    cacheLanguageCode.Add(code, language);
                }
            }
        }

        #endregion

        #region LanguageId

        public static Language CheckInCacheLanguageId(int id)
        {
            lock (lockObject)
            {
                if (cacheLanguageId.ContainsKey(id))
                {
                    return cacheLanguageId[id];
                }
            }
            return null;
        }

        public static void AddToCacheLanguageId(int id, Language language)
        {
            lock (lockObject)
            {
                if (cacheLanguageId.ContainsKey(id) == false)
                {
                    cacheLanguageId.Add(id, language);
                }
            }
        }

        #endregion

        #region DefaultLanguage

        public static Language CheckInCacheDefaultLanguage()
        {
            lock (lockObject)
            {
                if (cacheDefaultLanguage != null)
                {
                    return cacheDefaultLanguage;
                }
            }
            return null;
        }

        public static void AddToCacheDefaultLanguage(Language language)
        {
            lock (lockObject)
            {
                cacheDefaultLanguage = language;
            }
        }

        #endregion
    }
}
