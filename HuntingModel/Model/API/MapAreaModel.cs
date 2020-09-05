using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class MapAreaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PolygonData { get; set; }

        public MapAreaModel(MapArea mapArea)
        {
            this.Id = mapArea.Id;
            this.Name = mapArea.Name;
            this.PolygonData = mapArea.PolygonData;
        }
    }
}
