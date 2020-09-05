using HuntingModel.Infrastructure;
using HuntingModel.SqlGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AdminModels
{
    public class AclUserPageModel
    {
        public AclUserListModel ListModel { get; set; }
        public PagerModel Pager { get; set; }
        public AclUserFilter Filter { get; set; }

        public AclUserPageModel()
        {
        }

        public AclUserPageModel(AclUserListModel itemListModel, AclUserFilter filter)
        {
            this.ListModel = itemListModel;
            this.Pager = FilterUtils.GetPager(itemListModel.PageCount, itemListModel.PageIndex);
            this.Filter = filter;
        }
    }
}
