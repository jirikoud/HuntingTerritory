using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.MapItemTypeModels
{
    public class MapItemTypeListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MapItemTypeListItemModel(MapItemType itemType)
        {
            this.Id = itemType.Id;
            this.Name = itemType.Name;
            this.Description = itemType.Description;
        }
    }
}
