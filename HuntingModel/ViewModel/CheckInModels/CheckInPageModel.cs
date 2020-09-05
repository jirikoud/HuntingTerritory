using HuntingModel.Database;
using HuntingModel.Infrastructure;
using HuntingModel.SqlGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInPageModel
    {
        public int MapItemId { get; set; }
        public int TerritoryId { get; set; }
        public bool CanUpdate { get; set; }

        public CheckInListModel ListModel { get; set; }
        public PagerModel Pager { get; set; }
        public CheckInFilter Filter { get; set; }

        public CheckInPageModel()
        {
        }

        public CheckInPageModel(CheckInListModel itemListModel, CheckInFilter filter, MapItem mapItem)
        {
            this.MapItemId = mapItem.Id;
            this.TerritoryId = mapItem.TerritoryId;
            this.ListModel = itemListModel;
            this.Pager = FilterUtils.GetPager(itemListModel.PageCount, itemListModel.PageIndex);
            this.Filter = filter;
        }
    }
}
