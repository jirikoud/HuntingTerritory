using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.SqlGenerator.Generators
{
    public class AclUserFilter : FulltextFilterBase
    {
        public const string SORT_FULLNAME = "Fullname";
        public const string SORT_EMAIL = "Email";
        public const string SORT_IS_DISABLED = "IsDisabled";
        public const string SORT_ACCOUNT_TYPE = "AccountType";

        [Display(Name = "FILTER_ACCOUNT_TYPE", ResourceType = typeof(AdminRes))]
        public AccountTypeEnum? AccoutType { get; set; }
        public SelectList AccountTypeList { get; set; }

        public void PrepareFilter(int languageId)
        {
            this.AccountTypeList = AccountTypeEnumConvertor.GetSelectList(this.AccoutType, languageId);
        }
    }
}
