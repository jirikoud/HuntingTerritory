using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryUserContactModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }

        public TerritoryUserContactModel(TerritoryUserContact contact)
        {
            this.Id = contact.Id;
            this.UserName = contact.AclUser.Fullname;
            this.Message = contact.Message;
        }
    }
}
