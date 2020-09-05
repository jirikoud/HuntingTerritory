using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class RegisterModel
    {
        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AccountRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(250, ErrorMessageResourceName = "VALIDATION_MAX_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_PASSWORD", ResourceType = typeof(AccountRes))]
        [StringLength(30, MinimumLength =8, ErrorMessageResourceName = "VALIDATION_PASSWORD_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Compare("ConfirmPassword", ErrorMessageResourceName = "VALIDATION_PASSWORD_CONFIRM", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Password { get; set; }

        [Display(Name = "DETAIL_CONFIRM_PASSWORD", ResourceType = typeof(AccountRes))]
        public string ConfirmPassword { get; set; }
    }
}
