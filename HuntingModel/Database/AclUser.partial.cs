using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Database
{
    partial class AclUser
    {
        public AccountTypeEnum AccountTypeEx
        {
            get
            {
                return (AccountTypeEnum)this.AccountType;
            }
            set
            {
                this.AccountType = (int)value;
            }
        }

        public bool CanCreateTerritory()
        {
            if (this.AccountTypeEx == AccountTypeEnum.Standard)
            {
                return false;
            }
            if (this.MaxTerritoryCount < 0)
            {
                return true;
            }
            var currentCount = this.Territories.Where(item => item.IsDeleted == false).Count();
            return (currentCount < this.MaxTerritoryCount);
        }

        public bool CanViewTerritory(Territory territory)
        {
            if (territory == null)
            {
                return false;
            }
            if (this.AccountTypeEx == AccountTypeEnum.Admin)
            {
                return true;
            }
            if (territory.StewardId == this.Id)
            {
                return true;
            }
            if (territory.TerritoryUsers.Any(item => item.AclUserId == this.Id))
            {
                return true;
            }
            return false;
        }

        public bool CanUpdateTerritory(Territory territory)
        {
            if (territory == null)
            {
                return false;
            }
            if (this.AccountTypeEx == AccountTypeEnum.Admin)
            {
                return true;
            }
            if (territory.StewardId == this.Id)
            {
                return true;
            }
            return false;
        }

        public bool CanUpdateUserPoint(UserMapPoint userPoint)
        {
            if (this.AccountTypeEx == AccountTypeEnum.Admin)
            {
                return true;
            }
            if (userPoint.AclUserId == this.Id)
            {
                return true;
            }
            return false;
        }
    }
}
