using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuntingModel.ViewModel.AdminModels
{
    public class AclUserUpdateModel
    {
        public bool IsCreate { get; set; }
        public int Id { get; set; }

        [Display(Name = "DETAIL_EMAIL", ResourceType = typeof(AdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(250, ErrorMessageResourceName = "VALIDATION_MAX_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Email { get; set; }

        [Display(Name = "DETAIL_ACCOUNT_TYPE", ResourceType = typeof(AdminRes))]
        public AccountTypeEnum AccountType { get; set; }
        public SelectList AccountTypeSelectList { get; set; }

        [Display(Name = "DETAIL_MAX_TERRITORY_COUNT", ResourceType = typeof(AdminRes))]
        [RegularExpression(@"^-*[0-9]+$", ErrorMessageResourceName = "VALIDATION_NUMBER", ErrorMessageResourceType = typeof(ValidationRes))]
        public string MaxTerritoryCount { get; set; }

        [Display(Name = "DETAIL_FULLNAME", ResourceType = typeof(AdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(250, ErrorMessageResourceName = "VALIDATION_MAX_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Fullname { get; set; }

        public AclUserUpdateModel()
        {
        }

        public AclUserUpdateModel(AclUser aclUser)
        {
            this.Id = aclUser.Id;
            this.Email = aclUser.Email;
            this.AccountType = aclUser.AccountTypeEx;
            this.MaxTerritoryCount = aclUser.MaxTerritoryCount.ToString();
            this.Fullname = aclUser.Fullname;
        }

        public void FillCodeLists(int languageId)
        {
            this.AccountTypeSelectList = AccountTypeEnumConvertor.GetSelectList(this.AccountType, languageId);
        }
    }
}
