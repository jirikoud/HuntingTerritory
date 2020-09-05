using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Infrastructure
{
    public class ItemListHolder<T>
    {
        private int totalCount;
        private int pageSize;

        public List<T> ItemList { get; set; }

        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
            }
        }

        public int PageCount
        {
            get
            {
                var pageCount = totalCount / pageSize + (totalCount % pageSize > 0 ? 1 : 0);
                return pageCount;
            }
        }

        public ItemListHolder()
        {
            this.ItemList = new List<T>();
            this.pageSize = Constants.DEFAULT_LIST_PAGE_SIZE;
            this.totalCount = -1;
        }

        public ItemListHolder(List<T> itemList, int pageSize = Constants.DEFAULT_LIST_PAGE_SIZE)
        {
            this.ItemList = itemList ?? new List<T>();
            this.pageSize = pageSize;
            this.totalCount = -1;
        }
    }
}
