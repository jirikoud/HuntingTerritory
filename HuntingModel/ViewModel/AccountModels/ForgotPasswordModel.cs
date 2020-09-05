using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessageResourceName = "REQUIRED_FORGOTTEN_EMAIL", ErrorMessageResourceType = typeof(AccountRes))]
        public string Email { get; set; }
    }
}
