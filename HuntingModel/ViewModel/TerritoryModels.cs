using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuntingModel.Context;
using HuntingModel.Localization;
using HuntingModel.Model;

namespace HuntingModel.ViewModel
{
    public class TerritoryCreateViewModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "LABEL_NAME", ResourceType = typeof(TerritoryRes))]
        public string Name { get; set; }

        [StringLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "LABEL_DESCRIPTION", ResourceType = typeof(TerritoryRes))]
        public string Description { get; set; }
    }

    public class CoordinateViewModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public CoordinateViewModel()
        {
        }

        public CoordinateViewModel(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", Lat.ToString(CultureInfo.InvariantCulture), Lng.ToString(CultureInfo.InvariantCulture));
        }

    }

    public class HuntingGroundViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CoordinateViewModel> Polygon { get; set; }

        public HuntingGroundViewModel()
        {
            Polygon = new List<CoordinateViewModel>();
        }

        public HuntingGroundViewModel(HuntingGround huntingGround)
        {
            this.Id = huntingGround.Id;
            this.Name = huntingGround.Name;
            this.Polygon = TerritoryContext.ParseDatabasePolygonString(huntingGround.PolygonData);
        }

        public string PolygonToString()
        {
            var result = string.Join(",", this.Polygon);
            return result;
        }

    }

    public class MapItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CoordinateViewModel Coordinate { get; set; }

        public MapItemViewModel()
        {

        }

        public MapItemViewModel(MapItem mapItem)
        {
            this.Id = mapItem.Id;
            this.Name = mapItem.Name;
            this.Coordinate = new CoordinateViewModel(mapItem.LocationX, mapItem.LocationY);
        }
    }

    public class TerritoryDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<HuntingGroundViewModel> HuntingGroundList { get; set; }
        public List<MapItemViewModel> FeedingPlaceList { get; set; }
        public List<MapItemViewModel> ShootingPlaceList { get; set; }

        public TerritoryDetailViewModel()
        {
            this.HuntingGroundList = new List<HuntingGroundViewModel>();
            this.FeedingPlaceList = new List<MapItemViewModel>();
            this.ShootingPlaceList = new List<MapItemViewModel>();
        }

        public TerritoryDetailViewModel(Territory territory)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.HuntingGroundList = territory.HuntingGrounds.ToList().ConvertAll(item => new HuntingGroundViewModel(item));
            this.FeedingPlaceList = territory.MapItems.Where(item => item.MapItemType == 0).ToList().ConvertAll(item => new MapItemViewModel(item));
            this.ShootingPlaceList = territory.MapItems.Where(item => item.MapItemType == 1).ToList().ConvertAll(item => new MapItemViewModel(item));
        }
    }

    public class TerritoryListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TerritoryListItemViewModel()
        {

        }

        public TerritoryListItemViewModel(Territory territory)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
        }
    }

    public class TerritoryMapSavedViewModel
    {
        public bool IsSuccess { get; set; }
        public List<int> NewIdList { get; set; }

        public TerritoryMapSavedViewModel()
        {
            NewIdList = new List<int>();
        }

        public string NewIdString
        {
            get
            {
                return string.Join(",", this.NewIdList);
            }
        }
    }

    public class TerritoryListViewModel
    {
        public string SearchString { get; set; }
        public List<TerritoryListItemViewModel> ItemList { get; set; }
        public PagerViewModel Pager { get; set; }

        public TerritoryListViewModel()
        {

        }

        public TerritoryListViewModel(List<Territory> itemList, string searchString, int pageIndex, int pageCount)
        {
            this.SearchString = searchString;
            this.ItemList = itemList.ConvertAll(item => new TerritoryListItemViewModel(item));
            this.Pager = new PagerViewModel(pageIndex, pageCount);
        }
    }
}
