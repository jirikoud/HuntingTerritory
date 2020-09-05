using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.QuestionnaireModels
{
    public class QuestionnaireListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }

        public QuestionnaireListItemModel(Questionnaire questionnaire)
        {
            this.Id = questionnaire.Id;
            this.Name = questionnaire.Name;
            this.Description = questionnaire.Description;
            this.QuestionCount = questionnaire.Questions.Where(item => item.IsDeleted == false).Count();
        }
    }
}
