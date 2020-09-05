using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class QuestionnaireModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<QuestionModel> QuestionList { get; set; }

        public QuestionnaireModel(Questionnaire questionnaire)
        {
            this.Id = questionnaire.Id;
            this.Name = questionnaire.Name;
            this.Description = questionnaire.Description;

            var questionList = questionnaire.Questions.Where(item => item.IsDeleted == false).ToList();
            this.QuestionList = questionList.ConvertAll(item => new QuestionModel(item));
        }
    }
}
