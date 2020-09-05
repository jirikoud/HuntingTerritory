using HuntingModel.Context;
using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInUpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int MapItemId { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_CHECKIN_TIME", ResourceType = typeof(CheckInRes))]
        public string CheckTime { get; set; }
        public DateTime? CheckDateTime { get; set; }

        [Display(Name = "DETAIL_NOTE", ResourceType = typeof(CheckInRes))]
        public string Note { get; set; }

        [Display(Name = "DETAIL_QUESTIONNAIRE", ResourceType = typeof(CheckInRes))]
        public int? QuestionnaireId { get; set; }
        public SelectList QuestionnaireSelectList { get; set; }

        public List<CheckInQuestionModel> QuestionList { get; set; }

        public CheckInUpdateModel()
        {

        }

        public CheckInUpdateModel(CheckIn checkIn, Language language)
        {
            this.Id = checkIn.Id;
            this.MapItemId = checkIn.MapItemId;

            this.CheckTime = ContextUtils.FormatDateTime(checkIn.CheckTime, language, true);
            this.Note = checkIn.Note;
            this.QuestionnaireId = checkIn.QuestionnaireId;

            if (checkIn.QuestionnaireId.HasValue)
            {
                FillQuestionnaire(checkIn, checkIn.Questionnaire, language);
            }
        }

        public CheckInUpdateModel(Model.API.CreateCheckInModel checkIn, Questionnaire questionnaire)
        {
            this.IsCreate = true;
            this.MapItemId = checkIn.MapItemId;
            this.CheckDateTime = ContextUtils.DateTimeFromUnixTime(checkIn.CheckTime);
            this.Note = checkIn.Note;
            this.QuestionnaireId = checkIn.QuestionnaireId;
            if (checkIn.QuestionnaireId.HasValue)
            {
                this.QuestionList = checkIn.AnswerList.ConvertAll(item => new CheckInQuestionModel(item, questionnaire));
            }
        }

        public void FillCodeLists(HuntingEntities dataContext, MapItem mapItem)
        {
            if (mapItem != null)
            {
                this.QuestionnaireSelectList = QuestionnaireContext.GetSelectList(dataContext, mapItem.MapItemTypeRecord, this.QuestionnaireId);
            }
            if (this.QuestionList != null)
            {
                foreach (var questionModel in this.QuestionList)
                {
                    if (questionModel.QuestionType == QuestionTypeEnum.DropDown)
                    {
                        var question = dataContext.Questions.First(item => item.Id == questionModel.Id);
                        var optionList = question.Options.
                            Where(item => item.IsDeleted == false).
                            OrderBy(item => item.Order).
                            ToList();
                        questionModel.OptionList = new SelectList(optionList, "Id", "Name", questionModel.OptionId);
                    }
                }
            }
        }

        public void FillQuestionnaire(CheckIn checkIn, Questionnaire questionnaire, Language language)
        {
            var answerList = new List<Answer>();
            if (checkIn != null)
            {
                answerList = checkIn.Answers.ToList();
            }
            var questionList = questionnaire.Questions.Where(item => item.IsDeleted == false).OrderBy(item => item.Order).ToList();
            this.QuestionList = questionList.ConvertAll(item => new CheckInQuestionModel(item, answerList, language));
        }

    }
}
