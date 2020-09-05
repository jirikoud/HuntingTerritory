using HuntingModel.Database;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class ChangePasswordModel
    {
        public string Code { get; set; }

        [Display(Name = "DETAIL_FULLNAME", ResourceType = typeof(AdminRes))]
        public string UserName { get; set; }

        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AdminRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_PASSWORD", ResourceType = typeof(AccountRes))]
        [StringLength(30, MinimumLength = 8, ErrorMessageResourceName = "VALIDATION_PASSWORD_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Compare("ConfirmPassword", ErrorMessageResourceName = "VALIDATION_PASSWORD_CONFIRM", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Password { get; set; }

        [Display(Name = "DETAIL_CONFIRM_PASSWORD", ResourceType = typeof(AccountRes))]
        public string ConfirmPassword { get; set; }

        public ChangePasswordModel()
        {

        }

        public ChangePasswordModel(AclUser aclUser)
        {
            this.Code = aclUser.EmailCode;
            this.UserName = aclUser.Fullname;
            this.Email = aclUser.Email;
        }
    }
}
