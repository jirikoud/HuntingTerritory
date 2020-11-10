using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class ModelVersion
    {
        public long Id { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
    }
}
