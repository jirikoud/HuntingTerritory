using HuntingModel.Infrastructure;
using HuntingModel.SqlGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryPageModel
    {
        public TerritoryListModel ListModel { get; set; }
        public PagerModel Pager { get; set; }
        public TerritoryFilter Filter { get; set; }

        public TerritoryPageModel()
        {
        }

        public TerritoryPageModel(TerritoryListModel itemListModel, TerritoryFilter filter)
        {
            this.ListModel = itemListModel;
            this.Pager = FilterUtils.GetPager(itemListModel.PageCount, itemListModel.PageIndex);
            this.Filter = filter;
        }
    }
}
