using HuntingModel.Cache;
using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class LanguageContext
    {
        public const string LANG_CODE_CZ = "cs-CZ";
        public const string LANG_CODE_EN = "en-US";

        public static void CreateLanguages()
        {
            using (var dataContext = new HuntingEntities())
            {
                Console.WriteLine("CreateLanguages started");
                var langEnglish = dataContext.Languages.FirstOrDefault(item => item.Code == LANG_CODE_EN);
                if (langEnglish == null)
                {
                    langEnglish = new Language()
                    {
                        Code = LANG_CODE_EN,
                        Name = "English",
                        Shortcut = "en",
                        IsDefault = true,
                        Order = 1,
                        DateFormat = "MM/dd/yyyy",
                        TimeFormat = @"HH\:mm",
                        DateFormatJS = "m/d/Y",
                        TimeFormatJS = "H:i",
                    };
                    dataContext.Languages.Add(langEnglish);
                    Console.WriteLine("Language \"English\" created");
                }
                var langCzech = dataContext.Languages.FirstOrDefault(item => item.Code == LANG_CODE_CZ);
                if (langCzech == null)
                {
                    langCzech = new Language()
                    {
                        Code = LANG_CODE_CZ,
                        Name = "Čeština",
                        Shortcut = "cs",
                        Order = 2,
                        DateFormat = "dd.MM.yyyy",
                        TimeFormat = @"HH\:mm",
                        DateFormatJS = "d.m.Y",
                        TimeFormatJS = "H:i",
                    };
                    dataContext.Languages.Add(langCzech);
                    Console.WriteLine("Language \"Čeština\" created");
                }
                dataContext.SaveChanges();
                Console.WriteLine("CreateLanguages completed");
            }
        }

        public static List<Language> LoadLanguageList()
        {
            using (var context = new HuntingEntities())
            {
                var languageList = context.Languages.Where(item => item.IsDeleted == false).OrderBy(item => item.Order).ToList();
                return languageList;
            }
        }

        public static List<Language> GetLanguageList()
        {
            var cachedResult = LanguageCache.CheckInCacheLanguageList();
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var languageList = LoadLanguageList();
            LanguageCache.AddToCacheLanguageList(languageList);
            return languageList;
        }

        public static Language GetDefaultLanguage()
        {
            var cachedResult = LanguageCache.CheckInCacheDefaultLanguage();
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var languageList = GetLanguageList();
            var language = languageList.FirstOrDefault(item => item.IsDefault);
            LanguageCache.AddToCacheDefaultLanguage(language);
            return language;
        }

        public static Language GetLanguage(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return GetDefaultLanguage();
            }
            var cachedResult = LanguageCache.CheckInCacheLanguageCode(code);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var languageList = GetLanguageList();
            var language = languageList.FirstOrDefault(item => item.Code.ToLowerInvariant() == code.ToLowerInvariant());
            if (language == null)
            {
                language = GetDefaultLanguage();
            }
            LanguageCache.AddToCacheLanguageCode(code, language);
            return language;
        }

        public static Language GetLanguage(int id)
        {
            var cachedResult = LanguageCache.CheckInCacheLanguageId(id);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var languageList = GetLanguageList();
            var language = languageList.FirstOrDefault(item => item.Id == id);
            if (language == null)
            {
                language = GetDefaultLanguage();
            }
            LanguageCache.AddToCacheLanguageId(id, language);
            return language;
        }
    }
}
