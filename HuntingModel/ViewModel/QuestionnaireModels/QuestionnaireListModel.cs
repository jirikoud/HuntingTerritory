using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.QuestionnaireModels
{
    public class QuestionnaireListModel
    {
        public int MapItemTypeId { get; set; }
        public int TerritoryId { get; set; }
        public List<QuestionnaireListItemModel> ItemList { get; set; }

        public QuestionnaireListModel(MapItemType mapItemType, List<Questionnaire> itemList)
        {
            this.MapItemTypeId = mapItemType.Id;
            this.TerritoryId = mapItemType.TerritoryId;
            this.ItemList = itemList.ConvertAll(item => new QuestionnaireListItemModel(item));
        }
    }
}
