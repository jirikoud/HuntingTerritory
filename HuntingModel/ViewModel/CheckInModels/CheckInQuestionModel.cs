using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInQuestionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }

        public QuestionTypeEnum QuestionType { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public List<CheckInCheckBoxListItemModel> CheckBoxList { get; set; }
        public int? OptionId { get; set; }
        public SelectList OptionList { get; set; }

        public CheckInQuestionModel()
        {

        }

        public CheckInQuestionModel(Question question, List<Answer> answerList, Language language)
        {
            this.Id = question.Id;
            this.Name = question.Name;
            this.Description = question.Description;
            this.IsRequired = question.IsRequired;

            this.QuestionType = question.QuestionTypeEx;
            var localAnswerList = answerList.Where(item => item.QuestionId == question.Id).ToList();
            var singleAnswer = localAnswerList.FirstOrDefault();
            if (this.QuestionType == QuestionTypeEnum.TextArea || this.QuestionType == QuestionTypeEnum.TextBox)
            {
                this.StringValue = (singleAnswer != null ? singleAnswer.StringValue : null);
            }
            if (this.QuestionType == QuestionTypeEnum.Checkbox)
            {
                this.BoolValue = (singleAnswer != null ? (singleAnswer.BoolValue ?? false) : false);
            }
            if (this.QuestionType == QuestionTypeEnum.CheckboxList)
            {
                this.CheckBoxList = question.Options.
                    Where(item => item.IsDeleted == false).
                    OrderBy(item => item.Order).
                    ToList().
                    ConvertAll(item => new CheckInCheckBoxListItemModel(item, localAnswerList));
            }
            if (this.QuestionType == QuestionTypeEnum.DropDown)
            {
                this.OptionId = (singleAnswer != null ? singleAnswer.OptionId : null);
            }
            if (this.QuestionType == QuestionTypeEnum.Number)
            {
                this.StringValue = (singleAnswer != null ? ContextUtils.FormatFloat(singleAnswer.FloatValue, language) : null);
            }
        }

        public CheckInQuestionModel(Model.API.CheckInAnswerModel model, Questionnaire questionnaire)
        {
            var question = questionnaire.Questions.First(item => item.Id == model.QuestionId);
            this.Id = model.QuestionId;
            switch (question.QuestionTypeEx)
            {
                case QuestionTypeEnum.Checkbox:
                    this.BoolValue = model.BoolValue ?? false;
                    break;
                case QuestionTypeEnum.CheckboxList:
                    this.CheckBoxList = model.CheckBoxList.ConvertAll(item => new CheckInCheckBoxListItemModel(item));
                    break;
                case QuestionTypeEnum.DropDown:
                    this.OptionId = model.OptionId;
                    break;
                case QuestionTypeEnum.Number:
                    this.StringValue = (model.DoubleValue ?? 0).ToString();
                    break;
                case QuestionTypeEnum.TextArea:
                    this.StringValue = model.StringValue;
                    break;
                case QuestionTypeEnum.TextBox:
                    this.StringValue = model.StringValue;
                    break;
            }
        }

    }
}
