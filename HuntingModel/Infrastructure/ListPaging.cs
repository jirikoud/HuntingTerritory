using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Infrastructure
{
    public class ListPaging
    {
        private int innerPageSize { get; set; }
        public int PageIndex { get; set; }
        public bool TakeAll { get; set; }

        public int PageSize
        {
            get
            {
                if (TakeAll)
                {
                    return (innerPageSize * (PageIndex + 1));
                }
                return innerPageSize;
            }
        }

        public int SkipCount
        {
            get
            {
                if (TakeAll)
                {
                    return 0;
                }
                return (PageSize * PageIndex);
            }
        }

        public ListPaging(int pageSize, int pageIndex, bool? takeAll)
        {
            this.innerPageSize = pageSize;
            this.PageIndex = pageIndex;
            this.TakeAll = takeAll.HasValue ? takeAll.Value : false;
        }
    }
}
