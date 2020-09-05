using HuntingModel.Database;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HuntingModel.ViewModel.QuestionModels
{
    public class OptionUpdateModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Name { get; set; }

        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

        public OptionUpdateModel()
        {

        }

        public OptionUpdateModel(Option option)
        {
            this.Id = option.Id;
            this.Name = option.Name;
        }
    }
}