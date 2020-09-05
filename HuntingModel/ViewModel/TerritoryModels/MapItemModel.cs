using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class MapItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CoordinateModel Coordinate { get; set; }
        public int Index { get; set; }
        public string ItemType { get; set; }
        public int ItemTypeId { get; set; }

        public bool CanUpdate { get; set; }

        public string ToMapInitString(bool isUpdate, string mapItemArrayName)
        {
            return string.Format(@"
                {0}[{1}] = new google.maps.Marker({{
                    position: {2},
                    draggable: {5},
                    map: map,
                    title: '{3}',
                    description: '{4}',
                    itemtype: '{6}',
                    arrayindex: {1},
                    dbid: {7}, 
                }});",
                mapItemArrayName, 
                this.Index, 
                this.Coordinate.ToLatLngString(), 
                HttpUtility.HtmlEncode(this.Name),
                HttpUtility.HtmlEncode(this.Description),
                isUpdate ? "true" : "false",
                this.ItemType,
                this.Id);
        }

        public MapItemModel()
        {

        }

        public MapItemModel(MapItem mapItem)
        {
            this.Id = mapItem.Id;
            this.Name = mapItem.Name;
            this.Description = mapItem.Description;
            this.Coordinate = new CoordinateModel(mapItem.LocationX, mapItem.LocationY);
            this.ItemType = mapItem.MapItemType.ToString();
            this.ItemTypeId = mapItem.MapItemType;
        }

        public MapItemModel(UserMapPoint mapPoint, AclUser aclUser)
        {
            this.Id = mapPoint.Id;
            this.Name = mapPoint.Name;
            this.Description = mapPoint.Description;
            this.Coordinate = new CoordinateModel(mapPoint.LocationX, mapPoint.LocationY);
            this.CanUpdate = aclUser.CanUpdateUserPoint(mapPoint);
            this.ItemType = "user-point";
        }

        public MapItemModel(UserLocation userLocation)
        {
            this.Id = userLocation.Id;
            this.Name = userLocation.AclUser.Fullname;
            this.Description = null;
            this.Coordinate = new CoordinateModel(userLocation.LocationX, userLocation.LocationY);
            this.ItemType = "user-location";
        }
    }
}
