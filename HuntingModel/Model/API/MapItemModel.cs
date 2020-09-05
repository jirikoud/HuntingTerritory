using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class MapItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public MapItemModel(MapItem mapItem)
        {
            this.Id = mapItem.Id;
            this.Name = mapItem.Name;
            this.Description = mapItem.Description;
            this.LocationX = mapItem.LocationX;
            this.LocationY = mapItem.LocationY;
        }
    }
}
