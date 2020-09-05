using HuntingModel.Database;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.QuestionnaireModels
{
    public class QuestionnaireUpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int MapItemTypeId { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(QuestionnaireRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(QuestionnaireRes))]
        public string Description { get; set; }

        public QuestionnaireUpdateModel()
        {

        }

        public QuestionnaireUpdateModel(Questionnaire questionnaire)
        {
            this.Id = questionnaire.Id;
            this.MapItemTypeId = questionnaire.MapItemTypeId;
            this.Name = questionnaire.Name;
            this.Description = questionnaire.Description;
        }
    }
}
