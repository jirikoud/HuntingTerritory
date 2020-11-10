using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class UserMapPointShare
    {
        public long Id { get; set; }
        public long UserMapPointId { get; set; }
        public long AclUserId { get; set; }

    }
}
