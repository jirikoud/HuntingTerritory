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
    public class TerritoryUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public TerritoryUserRoleEnum Role { get; set; }
        public string RoleText { get; set; }
        public SelectList RoleSelectList { get; set; }

        public TerritoryUserModel()
        {

        }

        public TerritoryUserModel(TerritoryUser territoryUser, int languageId)
        {
            this.Id = territoryUser.Id;
            this.Email = territoryUser.AclUser.Email;
            this.Fullname = territoryUser.AclUser.Fullname;
            this.Role = territoryUser.UserRoleEx;
            this.RoleText = TerritoryUserRoleEnumConvertor.GetString(this.Role);
            this.RoleSelectList = TerritoryUserRoleEnumConvertor.GetSelectList(this.Role, languageId);
        }
    }
}
