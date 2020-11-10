using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Track
    {
        public long Id { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
    }
}
