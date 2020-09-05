using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
        public string Steward { get; set; }
        public bool CanContact { get; set; }

        public TerritoryListItemModel()
        {
        }

        public TerritoryListItemModel(Territory territory)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
        }

        public TerritoryListItemModel(TerritoryListItem territory)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.UserCount = territory.UserCount;
            this.Steward = territory.Steward;
            this.CanContact = (territory.ContactCount == 0);
        }
    }
}
