using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class CheckInAnswerModel
    {
        public int QuestionId { get; set; }
        public string StringValue { get; set; }
        public bool? BoolValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? OptionId { get; set; }
        public List<CheckInAnswerCheckBoxModel> CheckBoxList { get; set; }
    }
}
