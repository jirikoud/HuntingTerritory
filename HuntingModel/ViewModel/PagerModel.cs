using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel
{
    public class PageModel
    {
        public int PageIndex { get; set; }
        public bool IsSelected { get; set; }

        public PageModel(int pageIndex, bool isSelected = false)
        {
            this.PageIndex = pageIndex;
            this.IsSelected = isSelected;
        }
    }

    public class PagerModel
    {
        public List<PageModel> CenterPager { get; set; }
        public PageModel FirstPage { get; set; }
        public PageModel PrevPage { get; set; }
        public PageModel NextPage { get; set; }
        public PageModel LastPage { get; set; }
    }
}
