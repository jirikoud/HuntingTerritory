using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class MapItemSaveModel
    {
        public int? Id { get; set; }
        public int ItemType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CoordinateModel Position { get; set; }
        public bool IsDeleted { get; set; }
    }
}
