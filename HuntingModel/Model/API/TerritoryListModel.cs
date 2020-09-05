using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class TerritoryListModel
    {
        public List<TerritoryListItem> TerritoryList { get; set; }

        public TerritoryListModel(List<Territory> territoryList, AclUser aclUser)
        {
            this.TerritoryList = territoryList.ConvertAll(item => new TerritoryListItem(item, aclUser));
        }
    }
}
