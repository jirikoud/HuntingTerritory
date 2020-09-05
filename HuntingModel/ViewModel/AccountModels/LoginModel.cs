using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class LoginModel
    {
        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AccountRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_PASSWORD", ResourceType = typeof(AccountRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Password { get; set; }

        [Display(Name = "DETAIL_REMEMBER_ME", ResourceType = typeof(AccountRes))]
        public bool RememberMe{ get; set; }

        public bool CanDemo { get; set; }
    }
}
