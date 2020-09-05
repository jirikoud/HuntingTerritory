using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel
{
    public abstract class BaseListModel<TModel, TData>
    {
        public List<TModel> ListItem { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }

        public BaseListModel(ItemListHolder<TData> itemList, Func<TData, TModel> convert)
        {
            this.TotalCount = itemList.TotalCount;
            this.PageCount = itemList.PageCount;
            this.ListItem = itemList.ItemList.ConvertAll(item => convert(item));
        }
    }
}
