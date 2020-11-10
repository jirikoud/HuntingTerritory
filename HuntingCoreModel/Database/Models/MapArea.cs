﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class MapArea
    {
        public long Id { get; set; }
        public long TerritoryId { get; set; }
        public string Name { get; set; }
        public long PolygonData { get; set; }
        public long Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }
    }
}