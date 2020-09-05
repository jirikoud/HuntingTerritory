using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryListModel : BaseItemListModel<TerritoryListItemModel, TerritoryListItem>
    {
        public TerritoryListModel(ItemListHolder<TerritoryListItem> itemList, int pPageIndex)
            : base(itemList, item => new TerritoryListItemModel(item), pPageIndex)
        {
        }
    }
}
