using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.UserPointModels
{
    public class UserPointShareModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsShared { get; set; }

        public UserPointShareModel()
        { }

        public UserPointShareModel(TerritoryUser territoryUser, bool isShared)
        {
            this.Id = territoryUser.AclUser.Id;
            this.Name = territoryUser.AclUser.Fullname;
            this.IsShared = isShared;
        }
    }
}
