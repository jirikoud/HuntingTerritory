﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Questionnaire
    {
        public long Id { get; set; }
        public long MapItemTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }

        public List<Question> Questions { get; set; }
    }
}