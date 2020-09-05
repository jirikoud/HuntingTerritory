using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class UserLocationModel
    {
        public string UserName { get; set; }
        public long Created { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public UserLocationModel(UserLocation userLocation)
        {
            this.UserName = userLocation.AclUser.Fullname;
            this.Created = ContextUtils.DateTimeToUnixTime(userLocation.SysCreated);
            this.LocationX = userLocation.LocationX;
            this.LocationY = userLocation.LocationY;
        }
    }
}
