using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class CreateCheckInModel
    {
        public int MapItemId { get; set; }
        public int? QuestionnaireId { get; set; }
        public string Note { get; set; }
        public long CheckTime { get; set; }

        public List<CheckInAnswerModel> AnswerList { get; set; }

        public bool IsValid(HuntingEntities dataContext, Questionnaire questionnaire)
        {
            if (this.QuestionnaireId.HasValue && questionnaire == null)
            {
                return false;
            }
            if (questionnaire != null)
            {
                var questionLookup = questionnaire.Questions.Where(item => item.IsDeleted == false).ToDictionary(item => item.Id, item => item); 
                foreach (var answer in this.AnswerList)
                {
                    var question = questionLookup[answer.QuestionId];
                    if (question == null)
                    {
                        return false;
                    }
                    if (question.IsRequired)
                    {
                        switch (question.QuestionTypeEx)
                        {
                            case Enumeration.QuestionTypeEnum.DropDown:
                                if (answer.OptionId == null)
                                {
                                    return false;
                                }
                                break;
                            case Enumeration.QuestionTypeEnum.Number:
                                if (answer.DoubleValue == null)
                                {
                                    return false;
                                }
                                break;
                            case Enumeration.QuestionTypeEnum.TextArea:
                            case Enumeration.QuestionTypeEnum.TextBox:
                                if (string.IsNullOrWhiteSpace(answer.StringValue))
                                {
                                    return false;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }
    }
}
