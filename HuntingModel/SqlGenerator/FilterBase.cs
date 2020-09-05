using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator
{
    public class FilterBase
    {
        public string SortField { get; set; }
        public bool SortIsDesc { get; set; }
    }
}
