using HuntingModel.Context;
using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingWebJob
{
    internal class DemoAccountJob
    {
        public void ProcessJob()
        {
            using (var dataContext = new HuntingEntities())
            {
                var systemAccount = AclUserContext.GetAdminAccount(dataContext);
                TerritoryContext.RefreshDemoAccount(dataContext, systemAccount);
            }
        }
    }
}
