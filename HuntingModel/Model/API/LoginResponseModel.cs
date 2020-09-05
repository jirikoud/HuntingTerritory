using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class LoginResponseModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserSession { get; set; }

        public LoginResponseModel(UserSession session)
        {
            this.Email = session.AclUser.Email;
            this.FullName = session.AclUser.Fullname;
            this.UserSession = session.Session;
        }
    }
}
