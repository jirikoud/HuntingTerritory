using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.MapItemTypeModels
{
    public class MapItemTypeListModel
    {
        public int Id { get; set; }
        public List<MapItemTypeListItemModel> ItemTypeList { get; set; }

        public MapItemTypeListModel(Territory territory, List<MapItemType> itemTypeList)
        {
            this.Id = territory.Id;
            this.ItemTypeList = itemTypeList.ConvertAll(item => new MapItemTypeListItemModel(item));
        }
    }
}
