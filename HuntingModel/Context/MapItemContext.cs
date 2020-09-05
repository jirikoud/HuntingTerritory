using HuntingModel.Database;
using HuntingModel.Model.API;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class MapItemContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static MapItem GetDetail(HuntingEntities dataContext, int id)
        {
            try
            {
                var mapItem = dataContext.MapItems.FirstOrDefault(item => item.Id == id && item.IsDeleted == false);
                return mapItem;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static List<CheckIn> GetCheckInList(HuntingEntities dataContext, MapItem mapItem)
        {
            try
            {
                var checkInList = mapItem.CheckIns.Where(item => item.IsDeleted == false).ToList();
                return checkInList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetDetail");
                return null;
            }
        }

        public static int? Create(HuntingEntities dataContext, CreateMapItemModel model, int userId)
        {
            try
            {
                MapItem updateItem = new MapItem()
                {
                    SysCreated = DateTime.Now,
                    SysCreator = userId,
                    TerritoryId = model.TerritoryId,
                    MapItemType = model.MapItemTypeId,
                };
                dataContext.MapItems.Add(updateItem);

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
                updateItem.LocationX = model.LocationX;
                updateItem.LocationY = model.LocationY;
                updateItem.SysEditor = userId;
                updateItem.SysUpdated = DateTime.Now;

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Create");
                return null;
            }
        }
    }
}
