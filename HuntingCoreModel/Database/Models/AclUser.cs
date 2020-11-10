using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class AclUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public DateTime? SysUpdated { get; set; }
        public int? SysEditor { get; set; }
        public int AccountType { get; set; }
        public int MaxTerritoryCount { get; set; }  
        public string Fullname { get; set; }
        public string EmailCode { get; set; }
        public DateTime? EmailCodeExpire { get; set; }
    }
}
