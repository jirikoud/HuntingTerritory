using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel
{
    public class PagerViewModel
    {
        public bool IsPrevPage { get; set; }
        public bool IsNextPage { get; set; }

        public int PageIndexFirst { get; set; }
        public int PageIndexPrev { get; set; }
        public int PageIndexNext { get; set; }
        public int PageIndexLast { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }

        public PagerViewModel(int pageIndex, int totalCount)
        {
            this.PageIndex = pageIndex;
            this.CurrentPage = (pageIndex + 1);
            this.TotalPage = totalCount;

            var lastIndex = totalCount - 1;
            IsPrevPage = pageIndex > 0;
            IsNextPage = pageIndex < lastIndex;

            PageIndexFirst = 0;
            PageIndexPrev = (pageIndex == 0 ? 0 : pageIndex - 1);
            PageIndexNext = (pageIndex < lastIndex ? pageIndex + 1 : lastIndex);
            PageIndexLast = lastIndex;
        }
    }
}
