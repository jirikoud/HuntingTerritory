using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryMapSavedModel
    {
        public bool IsSuccess { get; set; }
        public List<int> MapAreaIdList { get; set; }
        public List<int> MapItemIdList { get; set; }

        public TerritoryMapSavedModel()
        {
            MapAreaIdList = new List<int>();
            MapItemIdList = new List<int>();
        }

        public string MapAreaIdString
        {
            get
            {
                return string.Join(",", this.MapAreaIdList);
            }
        }

        public string MapItemIdString
        {
            get
            {
                return string.Join(",", this.MapItemIdList);
            }
        }
    }

}
