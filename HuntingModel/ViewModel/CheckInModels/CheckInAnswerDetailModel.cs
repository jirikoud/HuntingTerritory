using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInAnswerDetailModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public List<CheckInCheckListDetailModel> CheckBoxList { get; set; }

        public CheckInAnswerDetailModel(Question question, List<Answer> answerList, Language language)
        {
            this.Name = question.Name;
            this.Description = question.Description;
            this.QuestionType = question.QuestionTypeEx;
            var singleAnswer = answerList.FirstOrDefault(item => item.QuestionId == question.Id);
            if (this.QuestionType == QuestionTypeEnum.Checkbox)
            {
                this.BoolValue = (singleAnswer != null ? (singleAnswer.BoolValue ?? false) : false);
            }
            else if (this.QuestionType == QuestionTypeEnum.CheckboxList)
            {
                var optionList = question.Options.Where(item => item.IsDeleted == false).OrderBy(item => item.Order).ToList();
                this.CheckBoxList = optionList.ConvertAll(item => new CheckInCheckListDetailModel(item, answerList));
            }
            else if (this.QuestionType == QuestionTypeEnum.DropDown)
            {
                this.StringValue = (singleAnswer != null ? (singleAnswer.Option != null ? singleAnswer.Option.Name : null) : null);
            }
            else if (this.QuestionType == QuestionTypeEnum.Number)
            {
                this.StringValue = (singleAnswer != null ? ContextUtils.FormatFloat(singleAnswer.FloatValue, language) : null);
            }
            else if (this.QuestionType == QuestionTypeEnum.TextArea)
            {
                this.StringValue = (singleAnswer != null ? singleAnswer.StringValue.Replace(Environment.NewLine, "<br/>") : null);
            }
            else if (this.QuestionType == QuestionTypeEnum.TextBox)
            {
                this.StringValue = (singleAnswer != null ? singleAnswer.StringValue : null);
            }
        }
    }
}
