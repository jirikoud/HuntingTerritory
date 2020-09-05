using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuntingModel.Model;
using HuntingModel.ViewModel;
using NLog;

namespace HuntingModel.Context
{
    public class TerritoryContext
    {
        private const string POLYGON_KEY_ID = "id";
        private const string POLYGON_KEY_NAME = "name";
        private const string POLYGON_KEY_COORDS = "coords";

        private const int LIST_PAGE_SIZE = 2;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Territory GetTerritory(HuntingEntities dataContext, int id)
        {
            try
            {
                var territory = dataContext.Territories.FirstOrDefault(item => item.Id == id);
                return territory;
            }
            catch (Exception exception)
            {
                logger.ErrorException("GetTerritory", exception);
                return null;
            }
        }

        public static Territory Create(HuntingEntities dataContext, TerritoryCreateViewModel model, HunterUser user)
        {
            try
            {
                var territory = new Territory()
                {
                    StewardId = user.Id,
                    Name = model.Name,
                    Description = model.Description,
                };
                dataContext.Territories.Add(territory);
                dataContext.SaveChanges();
                return territory;
            }
            catch (Exception exception)
            {
                logger.ErrorException("CreateTerritory", exception);
                return null;
            }
        }

        private static List<HuntingGroundViewModel> ParsePolygonString(string polygonString)
        {
            var itemList = new List<HuntingGroundViewModel>();
            var itemArray = polygonString.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var polygonDesc in itemArray)
            {
                var itemModel = new HuntingGroundViewModel();
                var paramArray = polygonDesc.Split(';');
                foreach (var paramDesc in paramArray)
                {
                    var valueArray = paramDesc.Split(':');
                    var key = valueArray[0];
                    var value = valueArray[1];
                    if (key == POLYGON_KEY_ID)
                    {
                        itemModel.Id = int.Parse(value);
                    }
                    if (key == POLYGON_KEY_NAME)
                    {
                        itemModel.Name = value;
                    }
                    if (key == POLYGON_KEY_COORDS)
                    {
                        var coordArray = value.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var coordDesc in coordArray)
                        {
                            var floatArray = coordDesc.Split(',');
                            var lat = float.Parse(floatArray[0], CultureInfo.InvariantCulture);
                            var lng = float.Parse(floatArray[1], CultureInfo.InvariantCulture);
                            var coordModel = new CoordinateViewModel(lat, lng);
                            itemModel.Polygon.Add(coordModel);
                        }
                    }
                }
                itemList.Add(itemModel);
            }
            return itemList;
        }

        private static List<MapItemViewModel> ParseMarkerString(string markerString)
        {
            var itemList = new List<MapItemViewModel>();
            var itemArray = markerString.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var polygonDesc in itemArray)
            {
                var itemModel = new MapItemViewModel();
                var paramArray = polygonDesc.Split(';');
                foreach (var paramDesc in paramArray)
                {
                    var valueArray = paramDesc.Split(':');
                    var key = valueArray[0];
                    var value = valueArray[1];
                    if (key == POLYGON_KEY_ID)
                    {
                        itemModel.Id = int.Parse(value);
                    }
                    if (key == POLYGON_KEY_NAME)
                    {
                        itemModel.Name = value;
                    }
                    if (key == POLYGON_KEY_COORDS)
                    {
                        var floatArray = value.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var lat = float.Parse(floatArray[0], CultureInfo.InvariantCulture);
                        var lng = float.Parse(floatArray[1], CultureInfo.InvariantCulture);
                        itemModel.Coordinate = new CoordinateViewModel(lat, lng);
                    }
                }
                itemList.Add(itemModel);
            }
            return itemList;
        }

        public static TerritoryDetailViewModel ParseMapSaveString(string polygonString)
        {
            var model = new TerritoryDetailViewModel();
            var objectTypeArray = polygonString.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < objectTypeArray.Length; index++)
            {
                if (index == 0)
                {
                    model.HuntingGroundList = ParsePolygonString(objectTypeArray[index]);
                }
                if (index == 1)
                {
                    model.FeedingPlaceList = ParseMarkerString(objectTypeArray[index]);
                }
                if (index == 2)
                {
                    model.ShootingPlaceList = ParseMarkerString(objectTypeArray[index]);
                }
            }
            return model;
        }

        public static List<CoordinateViewModel> ParseDatabasePolygonString(string polygonString)
        {
            var coordinateList = new List<CoordinateViewModel>();
            var itemArray = polygonString.Split('(', ')');
            foreach (var coordDesc in itemArray)
            {
                if (coordDesc.Length < 3)
                {
                    continue;
                }
                var coordArray = coordDesc.Split(',');
                var coordModel = new CoordinateViewModel(double.Parse(coordArray[0], CultureInfo.InvariantCulture), double.Parse(coordArray[1], CultureInfo.InvariantCulture));
                coordinateList.Add(coordModel);
            }
            return coordinateList;
        }

        public static TerritoryMapSavedViewModel SaveMap(HuntingEntities dataContext, int territoryId, TerritoryDetailViewModel data)
        {
            var model = new TerritoryMapSavedViewModel();
            try
            {
                var addedGrounds = new List<HuntingGround>();
                var addedFeedingPlaces = new List<MapItem>();
                var addedShootingPlaces = new List<MapItem>();
                var territory = dataContext.Territories.First(item => item.Id == territoryId);
                foreach (var ground in territory.HuntingGrounds.ToList())
                {
                    if (data.HuntingGroundList.Any(item => item.Id == ground.Id) == false)
                    {
                        dataContext.HuntingGrounds.Remove(ground);
                    }
                }
                foreach (var groundModel in data.HuntingGroundList)
                {
                    HuntingGround ground = null;
                    if (groundModel.Id > 0)
                    {
                        ground = dataContext.HuntingGrounds.First(item => item.Id == groundModel.Id);
                    }
                    else
                    {
                        ground = new HuntingGround()
                        {
                            TerritoryId = territoryId,
                        };
                        dataContext.HuntingGrounds.Add(ground);
                        addedGrounds.Add(ground);
                    }
                    ground.Name = groundModel.Name;
                    ground.PolygonData = groundModel.PolygonToString();
                }
                foreach (var mapItemModel in data.FeedingPlaceList)
                {
                    MapItem mapItem = null;
                    if (mapItemModel.Id > 0)
                    {
                        mapItem = dataContext.MapItems.First(item => item.Id == mapItemModel.Id);
                    }
                    else
                    {
                        mapItem = new MapItem()
                        {
                            TerritoryId = territoryId,
                        };
                        dataContext.MapItems.Add(mapItem);
                        addedFeedingPlaces.Add(mapItem);
                    }
                    mapItem.Name = mapItemModel.Name;
                    mapItem.LocationX = mapItemModel.Coordinate.Lat;
                    mapItem.LocationY = mapItemModel.Coordinate.Lng;
                }
                dataContext.SaveChanges();
                model.NewIdList = addedGrounds.ConvertAll(item => item.Id);
                model.IsSuccess = true;
            }
            catch (Exception exception)
            {
                logger.ErrorException("SaveMap", exception);
            }
            return model;
        }

        public static List<Territory> GetList(HuntingEntities dataContext, int pageIndex, string searchString)
        {
            List<Territory> itemList = null;
            try
            {
                if (string.IsNullOrWhiteSpace(searchString) == false)
                {
                    itemList = dataContext.Territories.
                        Where(item => item.IsDeleted == false && item.Name.Contains(searchString)).
                        OrderBy(item => item.Name).
                        Skip(pageIndex * LIST_PAGE_SIZE).
                        Take(LIST_PAGE_SIZE).
                        ToList();
                }
                else
                {
                    itemList = dataContext.Territories.
                        Where(item => item.IsDeleted == false).
                        OrderBy(item => item.Name).
                        Skip(pageIndex * LIST_PAGE_SIZE).
                        Take(LIST_PAGE_SIZE).
                        ToList();
                }
            }
            catch (Exception exception)
            {
                logger.ErrorException("GetList", exception);
            }
            return itemList;
        }

        public static int GetPageCount(HuntingEntities dataContext, int pageIndex, string searchString)
        {
            int pageCount = 1;
            try
            {
                var itemCount = 0;
                if (string.IsNullOrWhiteSpace(searchString) == false)
                {
                    itemCount = dataContext.Territories.
                        Where(item => item.IsDeleted == false && item.Name.Contains(searchString)).
                        Count();
                }
                else
                {
                    itemCount = dataContext.Territories.
                        Where(item => item.IsDeleted == false).
                        Count();
                }
                pageCount = (int)Math.Ceiling((double)itemCount / (double)LIST_PAGE_SIZE);
                pageCount = pageCount == 0 ? 1 : pageCount;
            }
            catch (Exception exception)
            {
                logger.ErrorException("GetPageCount", exception);
            }
            return pageCount;
        }
    }
}
