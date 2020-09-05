using HuntingModel.Database;
using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryPersonListModel
    {
        public int Id { get; set; }

        public List<TerritoryUserModel> UserList { get; set; }

        public TerritoryPersonListModel()
        {

        }

        public TerritoryPersonListModel(Territory territory, int languageId)
        {
            this.Id = territory.Id;
            this.UserList = territory.TerritoryUsers.Select(item => new TerritoryUserModel(item, languageId)).ToList();
        }
    }
}
