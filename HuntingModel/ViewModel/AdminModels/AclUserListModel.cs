using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AdminModels
{
    public class AclUserListModel : BaseItemListModel<AclUserListItemModel, AclUserListItem>
    {
        public AclUserListModel(ItemListHolder<AclUserListItem> itemList, int pPageIndex)
            : base(itemList, item => new AclUserListItemModel(item), pPageIndex)
        {
        }
    }

}
