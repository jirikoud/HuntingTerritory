using HuntingModel.Database;
using HuntingModel.ViewModel.MapItemTypeModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class MapItemTypeContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<MapItemType> GetList(HuntingEntities dataContext, Territory territory)
        {
            try
            {
                var itemTypeList = dataContext.MapItemTypes.
                    Where(item => item.TerritoryId == territory.Id && item.IsDeleted == false).
                    OrderBy(item => item.Name).
                    ToList();
                return itemTypeList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetList");
                return null;
            }
        }

        public static MapItemType GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var mapItemType = dataContext.MapItemTypes.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return mapItemType;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, MapItemType updateItem, MapItemTypeUpdateModel model, AclUser user)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new MapItemType()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = user.Id,
                        TerritoryId = model.TerritoryId,
                    };
                    dataContext.MapItemTypes.Add(updateItem);
                }

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
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

        public static bool Delete(HuntingEntities dataContext, MapItemType updateItem, AclUser user)
        {
            try
            {
                updateItem.IsDeleted = true;
                updateItem.SysEditor = user.Id;
                updateItem.SysUpdated = DateTime.Now;
                foreach (var mapItem in updateItem.MapItems)
                {
                    mapItem.IsDeleted = true;
                    mapItem.SysEditor = user.Id;
                    mapItem.SysUpdated = DateTime.Now;
                }
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Delete");
                return false;
            }
        }

    }
}
