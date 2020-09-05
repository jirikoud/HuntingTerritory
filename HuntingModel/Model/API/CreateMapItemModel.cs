using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class CreateMapItemModel
    {
        public int TerritoryId { get; set; }
        public int MapItemTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return false;
            }
            if (this.Name.Length > 30)
            {
                return false;
            }
            return true;
        }

    }
}
