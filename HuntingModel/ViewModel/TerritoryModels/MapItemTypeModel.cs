using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class MapItemTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MapItemModel> ItemList { get; set; }

        public MapItemTypeModel()
        {

        }

        public MapItemTypeModel(MapItemType mapItemType)
        {
            this.Id = mapItemType.Id;
            this.Name = mapItemType.Name;
            this.ItemList = mapItemType.MapItems.Where(item => item.IsDeleted == false).ToList().
                ConvertAll(item => new MapItemModel(item));
        }
    }
}
