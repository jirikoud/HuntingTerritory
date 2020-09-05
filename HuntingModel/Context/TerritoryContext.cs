using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuntingModel.ViewModel;
using NLog;
using HuntingModel.Database;
using HuntingModel.ViewModel.TerritoryModels;
using HuntingModel.Localization;
using System.Web;
using HuntingModel.Enumeration;
using HuntingModel.SqlGenerator.Generators;
using HuntingModel.SqlGenerator;
using HuntingModel.Infrastructure;

namespace HuntingModel.Context
{
    public class TerritoryContext
    {
        private const string POLYGON_KEY_ID = "id";
        private const string POLYGON_KEY_NAME = "name";
        private const string POLYGON_KEY_TYPE = "type";
        private const string POLYGON_KEY_COORDS = "coords";

        private const int LIST_PAGE_SIZE = 2;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Territory GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var territory = dataContext.Territories.FirstOrDefault(item => item.IsDeleted == false && item.Id == id);
                return territory;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTerritory");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, Territory updateItem, TerritoryUpdateModel model, AclUser user)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new Territory()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                        StewardId = user.Id,
                    };

                    var mapItemTypeFeeding = new MapItemType()
                    {
                        Name = TerritoryRes.MAP_ITEM_TYPE_FEEDING,
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                    };
                    updateItem.MapItemTypes.Add(mapItemTypeFeeding);

                    var mapItemTypePool = new MapItemType()
                    {
                        Name = TerritoryRes.MAP_ITEM_TYPE_POOL,
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                    };
                    updateItem.MapItemTypes.Add(mapItemTypePool);

                    dataContext.Territories.Add(updateItem);
                }

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
                updateItem.IsPublic = model.IsPublic;
                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Update");
                return null;
            }
        }

        public static List<CoordinateModel> ParseDatabasePolygonString(string polygonString)
        {
            var coordinateList = new List<CoordinateModel>();
            var itemArray = polygonString.Split('(', ')');
            foreach (var coordDesc in itemArray)
            {
                if (coordDesc.Length < 3)
                {
                    continue;
                }
                var coordArray = coordDesc.Split(',');
                var coordModel = new CoordinateModel(double.Parse(coordArray[0], CultureInfo.InvariantCulture), double.Parse(coordArray[1], CultureInfo.InvariantCulture));
                coordinateList.Add(coordModel);
            }
            return coordinateList;
        }

        public static TerritoryMapSavedModel SaveMap(HuntingEntities dataContext, int territoryId, MapSaveDataModel model, int userId)
        {
            var saveModel = new TerritoryMapSavedModel();
            var mapAreaList = new List<MapArea>();
            var mapItemList = new List<MapItem>();
            try
            {
                var territory = dataContext.Territories.First(item => item.Id == territoryId);
                foreach (var mapAreaModel in model.MapAreaList)
                {
                    MapArea mapArea;
                    if (mapAreaModel.Id > 0)
                    {
                        mapArea = dataContext.MapAreas.First(item => item.Id == mapAreaModel.Id);
                    }
                    else
                    {
                        mapArea = new MapArea()
                        {
                            SysCreated = DateTime.Now,
                            SysCreator = userId,
                            TerritoryId = territoryId,
                        };
                        dataContext.MapAreas.Add(mapArea);
                    }
                    mapArea.Name = HttpUtility.HtmlDecode(mapAreaModel.Title);
                    mapArea.Description = HttpUtility.HtmlDecode(mapAreaModel.Description);
                    mapArea.PolygonData = string.Join(",", mapAreaModel.Polygon);
                    mapArea.IsDeleted = mapAreaModel.IsDeleted;
                    mapArea.SysEditor = userId;
                    mapArea.SysUpdated = DateTime.Now;
                    mapAreaList.Add(mapArea);
                }
                foreach (var mapItemModel in model.MapItemList)
                {
                    MapItem mapItem;
                    if (mapItemModel.Id > 0)
                    {
                        mapItem = dataContext.MapItems.First(item => item.Id == mapItemModel.Id);
                    }
                    else
                    {
                        mapItem = new MapItem()
                        {
                            MapItemType = mapItemModel.ItemType,
                            SysCreated = DateTime.Now,
                            SysCreator = userId,
                            TerritoryId = territoryId,
                        };
                        dataContext.MapItems.Add(mapItem);
                    }
                    mapItem.Name = HttpUtility.HtmlDecode(mapItemModel.Title);
                    mapItem.Description = HttpUtility.HtmlDecode(mapItemModel.Description);
                    mapItem.LocationX = mapItemModel.Position.Lat;
                    mapItem.LocationY = mapItemModel.Position.Lng;
                    mapItem.IsDeleted = mapItemModel.IsDeleted;
                    mapItem.SysEditor = userId;
                    mapItem.SysUpdated = DateTime.Now;
                    mapItemList.Add(mapItem);
                }
                dataContext.SaveChanges();
                saveModel.MapAreaIdList = mapAreaList.ConvertAll(item => item.Id);
                saveModel.MapItemIdList = mapItemList.ConvertAll(item => item.Id);
                saveModel.IsSuccess = true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "SaveMap");
            }
            return saveModel;
        }

        public static int GetTotalCount(HuntingEntities context, TerritoryFilter filter, int userId)
        {
            try
            {
                var generator = new TerritoryListGenerator(filter, userId);
                var totalCount = QueryGenerator.GetTotalCount(context, generator);
                return totalCount;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTotalCount");
                return 0;
            }
        }

        public static ItemListHolder<TerritoryListItem> GetList(HuntingEntities context, TerritoryFilter filter, int pageIndex, int userId)
        {
            try
            {
                var generator = new TerritoryListGenerator(filter, userId);
                var list = QueryGenerator.ReadPagedList<TerritoryListItem>(context, generator, pageIndex, Constants.DEFAULT_LIST_PAGE_SIZE);
                var itemHolder = new ItemListHolder<TerritoryListItem>(list);
                itemHolder.TotalCount = GetTotalCount(context, filter, userId);
                return itemHolder;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetList");
                return new ItemListHolder<TerritoryListItem>();
            }
        }

        private static void RemoveTerritory(HuntingEntities dataContext, Territory territory)
        {
            dataContext.MapAreas.RemoveRange(territory.MapAreas);
            foreach (var mapItem in territory.MapItems)
            {
                dataContext.CheckIns.RemoveRange(mapItem.CheckIns);
            }
            dataContext.MapItems.RemoveRange(territory.MapItems);
            dataContext.MapItemTypes.RemoveRange(territory.MapItemTypes);
            dataContext.TerritoryUsers.RemoveRange(territory.TerritoryUsers);
            foreach (var userMapPoint in territory.UserMapPoints)
            {
                dataContext.UserMapPointShares.RemoveRange(userMapPoint.UserMapPointShares);
            }
            dataContext.UserMapPoints.RemoveRange(territory.UserMapPoints);
            dataContext.Territories.Remove(territory);
        }

        private static Territory CopyDemoTerritory(HuntingEntities dataContext, Territory template, AclUser demoUser)
        {
            var territory = new Territory()
            {
                Name = template.Name,
                Description = template.Description,
                StewardId = demoUser.Id,
                SysCreated = DateTime.Now,
                SysCreator = demoUser.Id,
            };
            dataContext.Territories.Add(territory);
            var mapAreaList = template.MapAreas.Where(item => item.IsDeleted == false);
            foreach (var templateMapArea in mapAreaList)
            {
                var mapArea = new MapArea()
                {
                    Name = templateMapArea.Name,
                    PolygonData = templateMapArea.PolygonData,
                    SysCreated = DateTime.Now,
                    SysCreator = demoUser.Id,
                };
                territory.MapAreas.Add(mapArea);
            }
            var typeDictionary = new Dictionary<int, MapItemType>();
            var mapItemTypeList = template.MapItemTypes.Where(item => item.IsDeleted == false);
            foreach (var templateMapItemType in mapItemTypeList)
            {
                var mapItemType = new MapItemType()
                {
                    Name = templateMapItemType.Name,
                    Description = template.Description,
                    SysCreated = DateTime.Now,
                    SysCreator = demoUser.Id,
                };
                territory.MapItemTypes.Add(mapItemType);
                typeDictionary.Add(templateMapItemType.Id, mapItemType);
            }
            var mapItemList = template.MapItems.Where(item => item.IsDeleted == false);
            foreach (var templateMapItem in mapItemList)
            {
                var mapItem = new MapItem()
                {
                    Name = templateMapItem.Name,
                    LocationX = templateMapItem.LocationX,
                    LocationY = templateMapItem.LocationY,
                    SysCreated = DateTime.Now,
                    SysCreator = demoUser.Id,
                    MapItemTypeRecord = typeDictionary[templateMapItem.MapItemType],
                };
                territory.MapItems.Add(mapItem);
            }
            var userMapPointList = template.UserMapPoints.Where(item => item.IsDeleted == false);
            foreach (var templateUserMapPoint in userMapPointList)
            {
                var userMapPoint = new UserMapPoint()
                {
                    Name = templateUserMapPoint.Name,
                    LocationX = templateUserMapPoint.LocationX,
                    LocationY = templateUserMapPoint.LocationY,
                    SysCreated = DateTime.Now,
                    SysCreator = demoUser.Id,
                    AclUserId = demoUser.Id,
                };
                territory.UserMapPoints.Add(userMapPoint);
            }
            return territory;
        }

        public static void RefreshDemoAccount(HuntingEntities dataContext, AclUser adminUser)
        {
            Console.Out.WriteLine("RefreshDemoAccount started...");
            AclUser demoAccount = null;
            var demoAccountList = dataContext.AclUsers.Where(item => item.AccountType == (int)AccountTypeEnum.Demo && item.IsDeleted == false).ToList();
            bool isFirst = true;
            foreach (var account in demoAccountList)
            {
                if (isFirst)
                {
                    demoAccount = account;
                    isFirst = false;
                }
                else
                {
                    account.IsDeleted = true;
                    account.SysEditor = adminUser.Id;
                    account.SysUpdated = DateTime.Now;
                }
            }
            if (demoAccount == null)
            {
                demoAccount = new AclUser()
                {
                    AccountTypeEx = AccountTypeEnum.Demo,
                    Email = "Demo Account",
                    Fullname = "HuntingApp Demo Account",
                    MaxTerritoryCount = 3,
                    PasswordHash = "sha1:64000:18:k0gmJBelxQsPFbTQ4l4KRNVM339CTzzM:gIreZu+Q/QzFcO+BmAmQRbPC",
                    SysCreated = DateTime.Now,
                };
                dataContext.AclUsers.Add(demoAccount);
            }
            var territoryList = demoAccount.Territories.ToList();
            foreach (var territory in territoryList)
            {
                RemoveTerritory(dataContext, territory);
            }

            var template = adminUser.Territories.FirstOrDefault(item => item.IsDemoTemplate == true);
            if (template == null)
            {
                template = new Territory()
                {
                    Description = "Demo Territory Template",
                    Name = "Demo Template",
                    StewardId = adminUser.Id,
                    SysCreated = DateTime.Now,
                    SysCreator = adminUser.Id,
                    IsDemoTemplate = true,
                };
                dataContext.Territories.Add(template);
            }

            var demo = CopyDemoTerritory(dataContext, template, demoAccount);

            dataContext.SaveChanges();
        }

        #region Territory Users

        public static bool InviteUser(HuntingEntities dataContext, Territory territory, string email, string name, AclUser sender)
        {
            var isNewUser = false;
            var lowerUserName = email.Trim().ToLower();

            var aclUser = dataContext.AclUsers.FirstOrDefault(item => item.IsDeleted == false && item.Email == lowerUserName);
            if (aclUser == null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }
                aclUser = new AclUser()
                {
                    AccountTypeEx = AccountTypeEnum.Standard,
                    Email = email,
                    EmailCode = Guid.NewGuid().ToString(),
                    Fullname = name,
                    MaxTerritoryCount = 0,
                    SysCreated = DateTime.Now,
                    SysEditor = sender.Id,
                    SysUpdated = DateTime.Now,
                };
                dataContext.AclUsers.Add(aclUser);
                EmailContext.CreateInviteEmail(dataContext, aclUser, sender);

                isNewUser = true;
            }

            if (territory.TerritoryUsers.Any(item => item.AclUserId == aclUser.Id))
            {
                return true;
            }

            var territoryUser = new TerritoryUser()
            {
                AclUser = aclUser,
                UserRoleEx = isNewUser ? TerritoryUserRoleEnum.Invited : TerritoryUserRoleEnum.Member,
            };
            territory.TerritoryUsers.Add(territoryUser);
            dataContext.SaveChanges();
            return true;
        }

        public static bool UpdateUserList(HuntingEntities dataContext, Territory territory, TerritoryPersonListModel model)
        {
            try
            {
                foreach (var modelUser in model.UserList)
                {
                    var user = territory.TerritoryUsers.FirstOrDefault(item => item.Id == modelUser.Id);
                    if (user != null)
                    {
                        user.UserRoleEx = modelUser.Role;
                    }
                }
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UpdatePersonList");
                return false;
            }
        }

        public static bool DeleteUser(HuntingEntities dataContext, Territory territory, int territoryUserId, AclUser user)
        {
            try
            {
                var territoryUser = territory.TerritoryUsers.FirstOrDefault(item => item.Id == territoryUserId);
                if (territoryUser != null)
                {
                    dataContext.TerritoryUsers.Remove(territoryUser);
                }
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "DeleteUser");
                return false;
            }
        }

        public static List<Territory> GetListForUser(HuntingEntities dataContext, AclUser user)
        {
            try
            {
                var territoryList = dataContext.Territories.
                    Where(item => item.IsDeleted == false && (item.StewardId == user.Id || item.TerritoryUsers.
                        Any(territoryUser => territoryUser.AclUserId == user.Id))).
                    ToList();
                return territoryList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetListForUser");
                return null;
            }
        }


        #endregion

        #region TerritoryUserContact

        public static bool Contact(HuntingEntities dataContext, Territory territory, AclUser user, string message)
        {
            try
            {
                var contact = new TerritoryUserContact()
                {
                    AclUserId = user.Id,
                    Message = message,
                    SysCreated = DateTime.Now,
                    SysCreator = user.Id,
                };
                territory.TerritoryUserContacts.Add(contact);
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "UpdatePersonList");
                return false;
            }
        }

        public static TerritoryUserContact GetTerritoryUserContact(HuntingEntities dataContext, int id)
        {
            try
            {
                var userContact = dataContext.TerritoryUserContacts.FirstOrDefault(item => item.Id == id);
                return userContact;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTerritoryUserContact");
                return null;
            }
        }

        public static bool ContactAction(HuntingEntities dataContext, TerritoryUserContact contact, bool accept, AclUser user)
        {
            try
            {
                if (accept)
                {
                    var territoryUser = new TerritoryUser()
                    {
                        AclUserId = contact.AclUserId,
                        UserRoleEx = TerritoryUserRoleEnum.Member,
                    };
                    contact.Territory.TerritoryUsers.Add(territoryUser);
                }
                contact.IsDeleted = true;
                contact.SysEditor = user.Id;
                contact.SysUpdated = DateTime.Now;
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetTerritoryUserContact");
                return false;
            }
        }

        #endregion
    }
}
