using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Option
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }
    }
}
