using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AdminModels
{
    public class AclUserDetailModel
    {
        public int Id { get; set; }

        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AdminRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_IS_DISABLED", ResourceType = typeof(AdminRes))]
        public string IsDisabledText { get; set; }
        public bool IsDisabled { get; set; }

        [Display(Name = "DETAIL_ACCOUNT_TYPE", ResourceType = typeof(AdminRes))]
        public string AccountType { get; set; }

        [Display(Name = "DETAIL_MAX_TERRITORY_COUNT", ResourceType = typeof(AdminRes))]
        public int MaxTerritoryCount { get; set; }

        [Display(Name = "DETAIL_FULLNAME", ResourceType = typeof(AdminRes))]
        public string Fullname { get; set; }

        public AclUserDetailModel(AclUser aclUser)
        {
            this.Id = aclUser.Id;
            this.Email = aclUser.Email;
            this.IsDisabledText = aclUser.IsDisabled ? GlobalRes.BOOL_VALUE_TRUE : GlobalRes.BOOL_VALUE_FALSE;
            this.IsDisabled = aclUser.IsDisabled;
            this.AccountType = AccountTypeEnumConvertor.GetString(aclUser.AccountTypeEx);
            this.MaxTerritoryCount = aclUser.MaxTerritoryCount;
            this.Fullname = aclUser.Fullname;
        }
    }
}
