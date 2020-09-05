using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class ManageModel
    {
        public int Id { get; set; }

        [Display(Name = "DETAIL_FULLNAME", ResourceType = typeof(AdminRes))]
        public string Fullname { get; set; }

        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AdminRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_ACCOUNT_TYPE", ResourceType = typeof(AdminRes))]
        public string AccountType { get; set; }

        [Display(Name = "DETAIL_MAX_TERRITORY_COUNT", ResourceType = typeof(AdminRes))]
        public int UsedTerritoryCount { get; set; }
        public int MaxTerritoryCount { get; set; }

        public bool CanChangePassword { get; set; }

        public ManageModel(AclUser aclUser)
        {
            this.Id = aclUser.Id;
            this.Fullname = aclUser.Fullname;
            this.Email = aclUser.Email;
            this.AccountType = AccountTypeEnumConvertor.GetString(aclUser.AccountTypeEx);
            this.MaxTerritoryCount = aclUser.MaxTerritoryCount;
            this.UsedTerritoryCount = aclUser.Territories.Where(item => item.IsDeleted == false).Count();
            this.CanChangePassword = (aclUser.AccountTypeEx != AccountTypeEnum.Demo);
        }
    }
}
