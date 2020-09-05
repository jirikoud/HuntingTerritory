using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInCheckListDetailModel
    {
        public string Name { get; set; }
        public bool BoolValue { get; set; }

        public CheckInCheckListDetailModel(Option option, List<Answer> answerList)
        {
            this.Name = option.Name;
            var answer = answerList.FirstOrDefault(item => item.OptionId == option.Id);
            this.BoolValue = (answer != null);
        }
    }
}
