using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class TerritoryUser
    {
        public long Id { get; set; }
        public long AclUserId { get; set; }
        public long TerritoryId { get; set; }
        public int TerritoryUserRole { get; set; }
    }
}
