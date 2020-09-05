using HuntingModel.Database;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using HuntingModel.ViewModel.TerritoryModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.UserPointModels
{
    public class UserPointUpdateModel
    {
        public int Id { get; set; }
        public int TerritoryId { get; set; }
        public bool IsCreate { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(30, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(UserPointRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(UserPointRes))]
        public string Description { get; set; }

        [Display(Name = "DETAIL_LOCATION_X", ResourceType = typeof(UserPointRes))]
        public string LocationX { get; set; }
        public double LocationXDouble { get; set; }

        [Display(Name = "DETAIL_LOCATION_Y", ResourceType = typeof(UserPointRes))]
        public string LocationY { get; set; }
        public double LocationYDouble { get; set; }

        [Display(Name = "DETAIL_SHARE", ResourceType = typeof(UserPointRes))]
        public bool IsPublic { get; set; }

        public List<UserPointShareModel> UserShareList { get; set; }

        public List<MapAreaModel> MapAreaList { get; set; }
        public List<MapItemModel> MapItemList { get; set; }
        public List<MapItemModel> UserMapItemList { get; set; }

        public void Validate(ModelStateDictionary modelState)
        {
            var locationX = ContextUtils.ParseFloatString(this.LocationX);
            if (locationX == null)
            {
                modelState.AddModelError("LocationX", ValidationRes.VALIDATION_DOUBLE);
            }
            else
            {
                this.LocationXDouble = locationX.Value;
            }
            var locationY = ContextUtils.ParseFloatString(this.LocationY);
            if (locationY == null)
            {
                modelState.AddModelError("LocationY", ValidationRes.VALIDATION_DOUBLE);
            }
            else
            {
                this.LocationYDouble = locationY.Value;
            }
        }

        public void FillTerritoryInfo(Territory territory, AclUser aclUser)
        {
            this.MapAreaList = territory.MapAreas.Where(item => item.IsDeleted == false).Select(item => new MapAreaModel(item)).ToList();
            this.MapItemList = territory.MapItems.Where(item => item.IsDeleted == false).Select(item => new MapItemModel(item)).ToList();

            var sharedMapItems = territory.UserMapPoints.Where(item => 
                item.Id != this.Id && (item.IsPublic || item.AclUserId == aclUser.Id || item.UserMapPointShares.Any(shareItem => shareItem.AclUserId == aclUser.Id))).
                ToList();
            this.UserMapItemList = sharedMapItems.ConvertAll(item => new MapItemModel(item, aclUser));

            for (int mapAreaIndex = 0; mapAreaIndex < this.MapAreaList.Count; mapAreaIndex++)
            {
                this.MapAreaList[mapAreaIndex].Index = mapAreaIndex;
            }
            int index = 0;
            foreach (var mapItem in this.MapItemList)
            {
                mapItem.Index = index;
                index++;
            }
            foreach (var mapItem in this.UserMapItemList)
            {
                mapItem.Index = index;
                index++;
            }
        }

        public string ToMapInitString(string polygonArray, string mapPointArray)
        {
            var builder = new StringBuilder();
            foreach (var mapArea in this.MapAreaList)
            {
                builder.Append(mapArea.ToMapInitString(false, polygonArray));
            }
            foreach (var mapItem in this.MapItemList)
            {
                builder.Append(mapItem.ToMapInitString(false, mapPointArray));
            }
            foreach (var mapItem in this.UserMapItemList)
            {
                builder.Append(mapItem.ToMapInitString(false, mapPointArray));
            }
            return builder.ToString();
        }

        public UserPointUpdateModel()
        {
            this.UserShareList = new List<UserPointShareModel>();
        }

        public UserPointUpdateModel(UserMapPoint mapPoint)
        {
            this.Id = mapPoint.Id;
            this.TerritoryId = mapPoint.TerritoryId;
            this.Name = mapPoint.Name;
            this.Description = mapPoint.Description;
            this.LocationX = mapPoint.LocationX.ToString(CultureInfo.CurrentUICulture);
            this.LocationY = mapPoint.LocationY.ToString(CultureInfo.CurrentUICulture);
            this.IsPublic = mapPoint.IsPublic;
            var shareIdList = mapPoint.UserMapPointShares.Select(item => item.AclUserId);
            this.UserShareList = mapPoint.Territory.TerritoryUsers.
                Select(item => new UserPointShareModel(item, shareIdList.Contains(item.AclUserId))).ToList();
        }

        public UserPointUpdateModel(HuntingModel.Model.API.CreateUserPointModel model)
        {
            this.IsCreate = true;
            this.TerritoryId = model.TerritoryId;
            this.Name = model.Name;
            this.Description = model.Description;
            this.LocationXDouble = model.LocationX;
            this.LocationYDouble = model.LocationY;
            this.IsPublic = model.IsPublic;
            this.UserShareList = new List<UserPointShareModel>();
        }

    }
}
