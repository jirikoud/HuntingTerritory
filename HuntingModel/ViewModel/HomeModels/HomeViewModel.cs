using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.ViewModel.TerritoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.HomeModels
{
    public class HomeViewModel
    {
        public bool IsUserLogged { get; set; }
        public List<TerritoryListItemModel> StewardList { get; set; }
        public List<TerritoryListItemModel> HunterList { get; set; }
        public bool CanCreate { get; set; }
        public bool CanContact { get; set; }

        public HomeViewModel()
        {

        }

        public HomeViewModel(AclUser user)
        {
            this.IsUserLogged = true;
            this.StewardList = user.Territories.ToList().ConvertAll(item => new TerritoryListItemModel(item));
            this.HunterList = user.TerritoryUsers.ToList().ConvertAll(item => new TerritoryListItemModel(item.Territory));
            this.CanCreate = user.CanCreateTerritory();
            this.CanContact = (user.AccountTypeEx != AccountTypeEnum.Demo);
        }
    }
}
