using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Database
{
    partial class TerritoryUser
    {
        public TerritoryUserRoleEnum UserRoleEx
        {
            get
            {
                return (TerritoryUserRoleEnum)TerritoryUserRole;
            }
            set
            {
                this.TerritoryUserRole = (int)value;
            }
        }
    }
}
