using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Language
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Shortcut { get; set; }
        public bool IsDefault { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string DateFormatJS { get; set; }
        public string TimeFormatJS { get; set; }
        public bool IsDeleted { get; set; }
    }
}
