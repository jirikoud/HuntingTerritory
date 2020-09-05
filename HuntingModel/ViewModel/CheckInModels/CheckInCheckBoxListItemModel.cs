using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInCheckBoxListItemModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool BoolValue { get; set; }

        public CheckInCheckBoxListItemModel()
        {

        }

        public CheckInCheckBoxListItemModel(Option option, List<Answer> answerList)
        {
            this.Id = option.Id;
            this.Text = option.Name;
            this.BoolValue = answerList.Any(item => item.OptionId == option.Id);
        }

        public CheckInCheckBoxListItemModel(Model.API.CheckInAnswerCheckBoxModel model)
        {
            this.Id = model.OptionId;
            this.BoolValue = model.BoolValue;
        }
    }
}
