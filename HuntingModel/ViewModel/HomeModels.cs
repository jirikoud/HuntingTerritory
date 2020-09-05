using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuntingModel.Model;

namespace HuntingModel.ViewModel
{
    public class HomeViewModel
    {
        public bool IsUserLogged { get; set; }
        public List<TerritoryListItemViewModel> StewardList { get; set; }
        public List<TerritoryListItemViewModel> HunterList { get; set; }

        public HomeViewModel()
        {

        }

        public HomeViewModel(HunterUser user)
        {
            this.IsUserLogged = true;
            this.StewardList = user.Territories.ToList().ConvertAll(item => new TerritoryListItemViewModel(item));
            this.HunterList = user.TerritoryUsers.ToList().ConvertAll(item => new TerritoryListItemViewModel(item.Territory));
        }
    }
}
