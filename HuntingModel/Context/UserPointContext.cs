using HuntingModel.Database;
using HuntingModel.Infrastructure;
using HuntingModel.ViewModel.UserPointModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class UserPointContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static UserMapPoint GetUserPoint(HuntingEntities dataContext, int id)
        {
            try
            {
                var userPoint = dataContext.UserMapPoints.FirstOrDefault(item => item.Id == id);
                return userPoint;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetUserPoint");
                return null;
            }
        }

        public static int? Update(HuntingEntities dataContext, UserMapPoint updateItem, UserPointUpdateModel model, int userId)
        {
            try
            {
                if (model.IsCreate)
                {
                    updateItem = new UserMapPoint()
                    {
                        SysCreated = DateTime.Now,
                        SysCreator = userId,
                        TerritoryId = model.TerritoryId,
                        AclUserId = userId,
                    };
                    dataContext.UserMapPoints.Add(updateItem);
                }

                updateItem.Name = model.Name;
                updateItem.Description = model.Description;
                updateItem.LocationX = model.LocationXDouble;
                updateItem.LocationY = model.LocationYDouble;
                updateItem.IsPublic = model.IsPublic;
                updateItem.SysEditor = userId;
                updateItem.SysUpdated = DateTime.Now;

                dataContext.UserMapPointShares.RemoveRange(updateItem.UserMapPointShares);
                if (model.IsPublic == false)
                {
                    foreach (var userShare in model.UserShareList)
                    {
                        if (userShare.IsShared)
                        {
                            var share = new UserMapPointShare()
                            {
                                AclUserId = userShare.Id,
                            };
                            updateItem.UserMapPointShares.Add(share);
                        }
                    }
                }

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Update");
                return null;
            }
        }
    }
}
