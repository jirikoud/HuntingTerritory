using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class UserLocation
    {
        public long Id { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public long TrackId { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
    }
}
