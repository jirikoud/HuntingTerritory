using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AdminModels
{
    public class AclUserListItemModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string AccountType { get; set; }
        public bool IsDisabled { get; set; }
        public int CurrentTerritoryCount { get; set; }
        public string MaxTerritoryCount { get; set; }

        public AclUserListItemModel(AclUserListItem aclUser)
        {
            this.Id = aclUser.Id;
            this.Email = aclUser.Email;
            this.Fullname = aclUser.Fullname;
            this.AccountType = AccountTypeEnumConvertor.GetString((AccountTypeEnum)aclUser.AccountType);
            this.IsDisabled = aclUser.IsDisabled;
            this.CurrentTerritoryCount = aclUser.CurrentTerritoryCount;
            this.MaxTerritoryCount = (aclUser.MaxTerritoryCount >= 0 ? aclUser.MaxTerritoryCount.ToString() : AccountRes.LABEL_TERRITORY_COUNT_UNLIMITED);
        }
    }
}
