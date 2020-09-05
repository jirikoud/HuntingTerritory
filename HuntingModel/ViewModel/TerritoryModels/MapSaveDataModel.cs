using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class MapSaveDataModel
    {
        public List<MapAreaSaveModel> MapAreaList { get; set; }
        public List<MapItemSaveModel> MapItemList { get; set; }
    }
}
