using HuntingModel.Database;
using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class TerritoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSteward { get; set; }
        public bool IsEditor { get; set; }

        public TerritoryListItem(Territory territory, AclUser aclUser)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.Description = territory.Description;
            this.IsSteward = (territory.StewardId == aclUser.Id);
            this.IsEditor = territory.TerritoryUsers.Any(item => item.UserRoleEx == TerritoryUserRoleEnum.Editor && item.AclUserId == aclUser.Id);
        }
    }
}
