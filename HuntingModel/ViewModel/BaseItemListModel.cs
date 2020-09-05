using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel
{
    public abstract class BaseItemListModel<TModel, TData> : BaseListModel<TModel, TData>
    {
        public int PageIndex { get; set; }

        public BaseItemListModel(ItemListHolder<TData> itemList, Func<TData, TModel> convert, int pPageIndex)
            : base(itemList, convert)
        {
            this.PageIndex = pPageIndex;
        }
    }
}
