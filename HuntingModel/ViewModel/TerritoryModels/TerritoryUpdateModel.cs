using HuntingModel.Database;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.TerritoryModels
{
    public class TerritoryUpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(TerritoryRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(TerritoryRes))]
        public string Description { get; set; }

        [Display(Name = "DETAIL_IS_PUBLIC", ResourceType = typeof(TerritoryRes))]
        public bool IsPublic { get; set; }

        public TerritoryUpdateModel()
        {

        }

        public TerritoryUpdateModel(Territory territory)
        {
            this.Id = territory.Id;
            this.Name = territory.Name;
            this.Description = territory.Description;
            this.IsPublic = territory.IsPublic;
        }
    }

}
