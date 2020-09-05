using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class CheckInModel
    {
        public int Id { get; set; }
        public long CheckTime { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }

        public List<AnswerModel> AnswerList { get; set; }

        public CheckInModel(CheckIn checkIn)
        {
            this.Id = checkIn.Id;
            this.CheckTime = ContextUtils.DateTimeToUnixTime(checkIn.CheckTime);
            this.Note = checkIn.Note;
            this.UserName = checkIn.AclUserCreator.Fullname;
            if (checkIn.QuestionnaireId.HasValue)
            {
                this.AnswerList = new List<AnswerModel>();

                var answerLookup = checkIn.Answers.ToLookup(item => item.QuestionId);
                var questionList = checkIn.Questionnaire.Questions.Where(item => item.IsDeleted == false).OrderBy(item => item.Order).ToList();
                foreach (var question in questionList)
                {
                    var answerList = answerLookup[question.Id].ToList();
                    var answer = answerList.FirstOrDefault();
                    switch (question.QuestionTypeEx)
                    {
                        case Enumeration.QuestionTypeEnum.Checkbox:
                            this.AnswerList.Add(new AnswerModel(question, (answer != null ? answer.BoolValue : false)));
                            break;
                        case Enumeration.QuestionTypeEnum.CheckboxList:
                            this.AnswerList.Add(new AnswerModel(question, answerList));
                            break;
                        case Enumeration.QuestionTypeEnum.DropDown:
                            this.AnswerList.Add(new AnswerModel(question, (answer != null ? answer.Option : null)));
                            break;
                        case Enumeration.QuestionTypeEnum.Number:
                            this.AnswerList.Add(new AnswerModel(question, (answer != null ? answer.FloatValue : null)));
                            break;
                        case Enumeration.QuestionTypeEnum.TextArea:
                            this.AnswerList.Add(new AnswerModel(question, (answer != null ? answer.StringValue : null)));
                            break;
                        case Enumeration.QuestionTypeEnum.TextBox:
                            this.AnswerList.Add(new AnswerModel(question, (answer != null ? answer.StringValue : null)));
                            break;
                    }
                }
            }
        }
    }
}
