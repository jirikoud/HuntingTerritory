using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class UserSession
    {
        public long Id { get; set; }
        public string Session { get; set; }
        public long AclUserId { get; set; }
        public DateTime SysCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
