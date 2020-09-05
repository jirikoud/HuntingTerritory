using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator.Generators
{
    public class TerritoryFilter : FulltextFilterBase
    {
        public const string SORT_NAME = "Name";

        public bool IsContact { get; set; }

        public void PrepareFilter(int languageId)
        {
        }
    }
}
