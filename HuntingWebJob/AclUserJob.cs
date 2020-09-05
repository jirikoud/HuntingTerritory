using HuntingModel.Context;
using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingWebJob
{
    internal class AclUserJob
    {
        public void ProcessJob()
        {
            using (var dataContext = new HuntingEntities())
            {
                AclUserContext.ClearExpiredEmailCodes(dataContext);
            }
        }

    }
}
