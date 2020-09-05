using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInListModel : BaseItemListModel<CheckInListItemModel, CheckInListItem>
    {
        public CheckInListModel(ItemListHolder<CheckInListItem> itemList, int pPageIndex, Language language)
            : base(itemList, item => new CheckInListItemModel(item, language), pPageIndex)
        {
        }
    }
}
