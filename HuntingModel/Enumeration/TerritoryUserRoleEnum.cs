using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.Enumeration
{
    public enum TerritoryUserRoleEnum
    {
        Invited,
        Member,
        Editor,
    }

    public class TerritoryUserRoleEnumConvertor
    {
        private static object lockObject = new object();
        private static Dictionary<int, List<Tuple<string, string>>> selectDictionary = new Dictionary<int, List<Tuple<string, string>>>();

        public static string GetString(TerritoryUserRoleEnum state)
        {
            switch (state)
            {
                case TerritoryUserRoleEnum.Invited: return TerritoryRes.TERRITORY_USER_ROLE_INVITED;
                case TerritoryUserRoleEnum.Member: return TerritoryRes.TERRITORY_USER_ROLE_MEMBER;
                case TerritoryUserRoleEnum.Editor: return TerritoryRes.TERRITORY_USER_ROLE_EDITOR;
            }
            return null;
        }

        private static List<Tuple<string, string>> GetSelectListInner()
        {
            var list = new List<Tuple<string, string>>();
            list.Add(Tuple.Create(TerritoryUserRoleEnum.Member.ToString(), TerritoryRes.TERRITORY_USER_ROLE_MEMBER));
            list.Add(Tuple.Create(TerritoryUserRoleEnum.Editor.ToString(), TerritoryRes.TERRITORY_USER_ROLE_EDITOR));
            return list;
        }

        public static SelectList GetSelectList(TerritoryUserRoleEnum? selected, int languageId)
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
