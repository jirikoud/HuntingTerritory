using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.Enumeration
{
    public enum AccountTypeEnum
    {
        Standard,
        Payed,
        Admin,
        Demo,
    }

    public class AccountTypeEnumConvertor
    {
        private static object lockObject = new object();
        private static Dictionary<int, List<Tuple<string, string>>> selectDictionary = new Dictionary<int, List<Tuple<string, string>>>();

        public static string GetString(AccountTypeEnum state)
        {
            switch (state)
            {
                case AccountTypeEnum.Admin: return AccountRes.ACCOUNT_TYPE_ADMIN;
                case AccountTypeEnum.Payed: return AccountRes.ACCOUNT_TYPE_PAYING;
                case AccountTypeEnum.Standard: return AccountRes.ACCOUNT_TYPE_STANDARD;
                case AccountTypeEnum.Demo: return AccountRes.ACCOUNT_TYPE_DEMO;
            }
            return null;
        }

        private static List<Tuple<string, string>> GetSelectListInner()
        {
            var list = new List<Tuple<string, string>>();
            list.Add(Tuple.Create(AccountTypeEnum.Admin.ToString(), AccountRes.ACCOUNT_TYPE_ADMIN));
            list.Add(Tuple.Create(AccountTypeEnum.Payed.ToString(), AccountRes.ACCOUNT_TYPE_PAYING));
            list.Add(Tuple.Create(AccountTypeEnum.Standard.ToString(), AccountRes.ACCOUNT_TYPE_STANDARD));
            list.Add(Tuple.Create(AccountTypeEnum.Demo.ToString(), AccountRes.ACCOUNT_TYPE_DEMO));
            return list;
        }

        public static SelectList GetSelectList(AccountTypeEnum? selected, int languageId)
        {
            lock (lockObject)
            {
                if (selectDictionary.ContainsKey(languageId) == false)
                {
                    selectDictionary.Add(languageId, GetSelectListInner());
                }
                var selectList = new SelectList(selectDictionary[languageId], "Item1", "Item2", selected);
                return selectList;
            }
        }
    }
}
