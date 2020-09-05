using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInDetailModel
    {
        public int Id { get; set; }
        public int MapItemId { get; set; }

        public string CheckInTime { get; set; }
        public string Questionnaire { get; set; }
        public string Note { get; set; }

        public bool CanUpdate { get; set; }

        public List<CheckInAnswerDetailModel> QuestionList { get; set; }

        public CheckInDetailModel(CheckIn checkIn, Language language)
        {
            this.Id = checkIn.Id;
            this.MapItemId = checkIn.MapItemId;
            this.CheckInTime = ContextUtils.FormatDateTime(checkIn.CheckTime, language, true);
            this.Note = checkIn.Note;

            if (checkIn.QuestionnaireId.HasValue)
            {
                this.Questionnaire = checkIn.Questionnaire.Name;
                var answerList = checkIn.Answers.ToList();
                var questionList = checkIn.Questionnaire.Questions.
                    Where(item => item.IsDeleted == false).
                    OrderBy(item => item.Order).
                    ToList();
                this.QuestionList = questionList.ConvertAll(item => new CheckInAnswerDetailModel(item, answerList, language));
            }
        }
    }
}
