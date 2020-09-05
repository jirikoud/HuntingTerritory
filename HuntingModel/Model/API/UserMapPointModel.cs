using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class UserMapPointModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public UserMapPointModel(UserMapPoint userMapPoint)
        {
            this.Id = userMapPoint.Id;
            this.Name = userMapPoint.Name;
            this.Description = userMapPoint.Description;
            this.LocationX = userMapPoint.LocationX;
            this.LocationY = userMapPoint.LocationY;
        }
    }
}
