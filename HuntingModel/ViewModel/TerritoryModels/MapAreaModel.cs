using HuntingModel.Context;
using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class MapAreaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CoordinateModel> Polygon { get; set; }
        public int Index { get; set; }

        public MapAreaModel()
        {
            Polygon = new List<CoordinateModel>();
        }

        public MapAreaModel(MapArea mapArea)
        {
            this.Id = mapArea.Id;
            this.Name = mapArea.Name;
            this.Description = mapArea.Description;
            this.Polygon = TerritoryContext.ParseDatabasePolygonString(mapArea.PolygonData);
        }

        public string PolygonToString()
        {
            var result = string.Join(",", this.Polygon);
            return result;
        }

        public string ToMapInitString(bool isUpdate, string mapAreaArrayName)
        {
            return string.Format(@"
                {0}[{1}] = new google.maps.Polygon({{
                    paths: [{2}],
                    strokeColor: '#FF0000',
                    strokeOpacity: 0.8,
                    strokeWeight: 3,
                    fillColor: '#FF0000',
                    fillOpacity: 0.35,
                    editable: false,
                    draggable: false,
                    map: map,
                    title: '{3}',
                    description: '{4}',
                    dbid: {5}, 
                    arrayindex: {1},
                }});", 
                mapAreaArrayName, this.Index, string.Join(",\n", this.Polygon.Select(item => item.ToLatLngString())), 
                HttpUtility.HtmlEncode(this.Name), HttpUtility.HtmlEncode(this.Description), this.Id);
        }
    }
}
