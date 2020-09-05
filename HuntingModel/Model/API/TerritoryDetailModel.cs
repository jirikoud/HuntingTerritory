using HuntingModel.Database;
using HuntingModel.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class TerritoryDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<MapAreaModel> MapAreaList { get; set; }
        public List<MapItemTypeModel> MapItemTypeList { get; set; }
        public List<UserMapPointModel> UserPointList { get; set; }

        public TerritoryDetailModel(Territory territory, AclUser user)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.Description = territory.Description;

            var mapAreaList = territory.MapAreas.Where(item => item.IsDeleted == false).ToList();
            this.MapAreaList = mapAreaList.ConvertAll(item => new MapAreaModel(item));

            var mapItemTypeList = territory.MapItemTypes.Where(item => item.IsDeleted == false).ToList();
            this.MapItemTypeList = mapItemTypeList.ConvertAll(item => new MapItemTypeModel(item));

            var userPointList = territory.UserMapPoints.
                Where(item => item.IsDeleted == false && 
                (item.AclUserId == user.Id || item.IsPublic == true || item.UserMapPointShares.Any(share => share.AclUserId == user.Id))).
                ToList();
            this.UserPointList = userPointList.ConvertAll(item => new UserMapPointModel(item));
        }
    }
}
