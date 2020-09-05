using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.QuestionModels
{
    public class QuestionUpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int QuestionnaireId { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(QuestionRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(QuestionRes))]
        public string Description { get; set; }

        [Display(Name = "DETAIL_QUESTION_TYPE", ResourceType = typeof(QuestionRes))]
        public QuestionTypeEnum QuestionType { get; set; }
        public SelectList QuestionTypeSelectList { get; set; }

        public List<OptionUpdateModel> OptionList { get; set; }

        [Display(Name = "DETAIL_IS_REQUIRED", ResourceType = typeof(QuestionRes))]
        public bool IsRequired { get; set; }

        public QuestionUpdateModel()
        {
            this.OptionList = new List<OptionUpdateModel>();
        }

        public QuestionUpdateModel(Question question)
        {
            this.Id = question.Id;
            this.QuestionnaireId = question.QuestionnaireId;
            this.Name = question.Name;
            this.Description = question.Description;
            this.QuestionType = question.QuestionTypeEx;
            this.OptionList = question.Options.
                Where(item => item.IsDeleted == false).
                OrderBy(item => item.Order).
                Select(item => new OptionUpdateModel(item)).
                ToList();
            this.IsRequired = question.IsRequired;
        }

        public void FillCodeList(int languageId)
        {
            this.QuestionTypeSelectList = QuestionTypeEnumConvertor.GetSelectList(this.QuestionType, languageId);
            if (this.OptionList.Count > 0)
            {
                this.OptionList.First().IsFirst = true;
                this.OptionList.Last().IsLast = true;
            }
        }
    }
}
