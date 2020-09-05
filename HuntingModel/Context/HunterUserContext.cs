using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HuntingModel.Model;
using NLog;

namespace HuntingModel.Context
{
    public class HunterUserContext
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static HunterUser GetUserByName(HuntingEntities dataContext, string userId)
        {
            try
            {
                var user = dataContext.HunterUsers.FirstOrDefault(item => item.Id == userId);
                if (user == null)
                {
                    user = new HunterUser()
                    {
                        Id = userId,
                    };
                    dataContext.HunterUsers.Add(user);
                    dataContext.SaveChanges();
                }
                return user;
            }
            catch(Exception exception)
            {
                logger.ErrorException("GetUserByName", exception);
                return null;
            }
        }

    }
}
