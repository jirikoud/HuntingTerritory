using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Territory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StewardId { get; set; }
        public bool IsDemoTemplate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }
    }
}
