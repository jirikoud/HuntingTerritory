using HuntingModel.Database;
using HuntingModel.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class UserLocationContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static int? CreateTrack(HuntingEntities dataContext, int userId)
        {
            try
            {
                var updateItem = new Track()
                {
                    SysCreated = DateTime.Now,
                    SysCreator = userId,
                };
                dataContext.Tracks.Add(updateItem);

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CreateTrack");
                return null;
            }
        }

        public static List<UserLocation> GetUserLocationList(HuntingEntities dataContext, Territory territory)
        {
            try
            {
                var userLocationList = new List<UserLocation>();
                var stewardLocation = territory.AclUserSteward.UserLocations.OrderByDescending(item => item.SysCreated).FirstOrDefault();
                if (stewardLocation != null && stewardLocation.SysCreated > DateTime.Now.AddMinutes(-Settings.Default.UserLocationLastMinutes))
                {
                    userLocationList.Add(stewardLocation);
                }
                foreach (var territoryUser in territory.TerritoryUsers)
                {
                    var userLocation = territoryUser.AclUser.UserLocations.OrderByDescending(item => item.SysCreated).FirstOrDefault();
                    if (userLocation != null && userLocation.SysCreated > DateTime.Now.AddMinutes(-Settings.Default.UserLocationLastMinutes))
                    {
                        userLocationList.Add(userLocation);
                    }
                }
                return userLocationList;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "GetUserLocationList");
                return null;
            }
        }

        public static int? CreateUserLocation(HuntingEntities dataContext, int trackId, double locationX, double locationY, int userId)
        {
            try
            {
                var updateItem = new UserLocation()
                {
                    TrackId = trackId,
                    SysCreated = DateTime.Now,
                    SysCreator = userId,
                    LocationX = locationX,
                    LocationY = locationY,
                };
                dataContext.UserLocations.Add(updateItem);

                dataContext.SaveChanges();
                return updateItem.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "CreateUserLocation");
                return null;
            }
        }

    }
}
