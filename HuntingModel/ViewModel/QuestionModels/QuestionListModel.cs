using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.QuestionModels
{
    public class QuestionListModel
    {
        public int QuestionnaireId { get; set; }
        public int MapItemTypeId { get; set; }
        public List<QuestionListItemModel> ItemList { get; set; }

        public QuestionListModel(Questionnaire questionnaire, List<Question> itemList)
        {
            this.QuestionnaireId = questionnaire.Id;
            this.MapItemTypeId = questionnaire.MapItemTypeId;
            this.ItemList = itemList.ConvertAll(item => new QuestionListItemModel(item));
            if (this.ItemList.Count > 0)
            {
                this.ItemList.First().IsFirst = true;
                this.ItemList.Last().IsLast = true;
            }
        }
    }
}
