using HuntingModel.Database;
using HuntingModel.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MapAreaModel> MapAreaList { get; set; }
        public List<MapItemTypeModel> MapItemTypeList { get; set; }
        public List<MapItemModel> UserMapItemList { get; set; }
        public List<MapItemModel> UserLocationList { get; set; }

        public bool CanUpdate { get; set; }

        public List<TerritoryUserContactModel> ContactList { get; set; }

        private void PrepareMapItemIndex()
        {
            for (int polygonIndex = 0; polygonIndex < this.MapAreaList.Count; polygonIndex++)
            {
                this.MapAreaList[polygonIndex].Index = polygonIndex;
            }

            int index = 0;
            foreach (var mapItemType in this.MapItemTypeList)
            {
                foreach (var mapItem in mapItemType.ItemList)
                {
                    mapItem.Index = index;
                    index++;
                }
            }
            foreach (var mapItem in this.UserMapItemList)
            {
                mapItem.Index = index;
                index++;
            }
            foreach (var mapItem in this.UserLocationList)
            {
                mapItem.Index = index;
                index++;
            }
        }

        public string ToMapInitString(bool isUpdate, string mapAreaArrayName, string mapItemArrayName)
        {
            var builder = new StringBuilder();
            foreach (var mapArea in this.MapAreaList)
            {
                builder.Append(mapArea.ToMapInitString(isUpdate, mapAreaArrayName));
            }
            foreach (var mapItemType in this.MapItemTypeList)
            {
                foreach (var mapItem in mapItemType.ItemList)
                {
                    builder.Append(mapItem.ToMapInitString(isUpdate, mapItemArrayName));
                }
            }
            if (isUpdate == false)
            {
                foreach (var mapItem in this.UserMapItemList)
                {
                    builder.Append(mapItem.ToMapInitString(isUpdate, mapItemArrayName));
                }
                foreach (var mapItem in this.UserLocationList)
                {
                    builder.Append(mapItem.ToMapInitString(isUpdate, mapItemArrayName));
                }
            }
            return builder.ToString();
        }

        public TerritoryDetailModel()
        {
            this.MapAreaList = new List<MapAreaModel>();
            this.MapItemTypeList = new List<MapItemTypeModel>();
        }

        public TerritoryDetailModel(Territory territory, AclUser aclUser)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.Description = territory.Description;
            this.MapAreaList = territory.MapAreas.Where(item => item.IsDeleted == false).ToList().ConvertAll(item => new MapAreaModel(item));
            this.MapItemTypeList = territory.MapItemTypes.Where(item => item.IsDeleted == false).ToList().ConvertAll(item => new MapItemTypeModel(item));

            var sharedMapItems = territory.UserMapPoints.
                Where(item => item.IsDeleted == false && (item.IsPublic || item.AclUserId == aclUser.Id || item.UserMapPointShares.Any(shareItem => shareItem.AclUserId == aclUser.Id))).
                OrderBy(item => item.Name).ToList();
            this.UserMapItemList = sharedMapItems.ConvertAll(item => new MapItemModel(item, aclUser));

            this.UserLocationList = new List<MapItemModel>();
            var stewardLocation = territory.AclUserSteward.UserLocations.OrderByDescending(item => item.SysCreated).FirstOrDefault();
            if (stewardLocation != null && stewardLocation.SysCreated > DateTime.Now.AddMinutes(-Settings.Default.UserLocationLastMinutes))
            {
                this.UserLocationList.Add(new MapItemModel(stewardLocation));
            }
            foreach (var territoryUser in territory.TerritoryUsers)
            {
                var userLocation = territoryUser.AclUser.UserLocations.OrderByDescending(item => item.SysCreated).FirstOrDefault();
                if (userLocation != null && userLocation.SysCreated > DateTime.Now.AddMinutes(-Settings.Default.UserLocationLastMinutes))
                {
                    this.UserLocationList.Add(new MapItemModel(userLocation));
                }
            }
            if (territory.StewardId == aclUser.Id)
            {
                this.ContactList = territory.TerritoryUserContacts.
                    Where(item => item.IsDeleted == false).
                    Select(item => new TerritoryUserContactModel(item)).ToList();
            }

            PrepareMapItemIndex();
            this.CanUpdate = aclUser.CanUpdateTerritory(territory);
        }
    }
}
