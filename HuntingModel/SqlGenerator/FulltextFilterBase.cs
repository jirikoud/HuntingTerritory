using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator
{
    public class FulltextFilterBase : FilterBase
    {
        public const string SORT_RANK = "ftrank";

        [Display(Name = "FILTER_FULLTEXT", ResourceType = typeof(GlobalRes))]
        public string Fulltext { get; set; }
    }
}
