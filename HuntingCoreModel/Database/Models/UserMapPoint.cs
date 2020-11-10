using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class UserMapPoint
    {
        public long Id { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public long TerritoryId { get; set; }
        public long AclUserId { get; set; }
    }
}
