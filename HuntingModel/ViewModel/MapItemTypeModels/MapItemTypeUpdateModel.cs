using HuntingModel.Database;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.MapItemTypeModels
{
    public class MapItemTypeUpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int TerritoryId { get; set; }

        [Required(ErrorMessageResourceName = "VALIDATION_REQUIRED", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "VALIDATION_STRING_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(MapItemTypeRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(MapItemTypeRes))]
        public string Description { get; set; }

        public MapItemTypeUpdateModel()
        {

        }

        public MapItemTypeUpdateModel(MapItemType mapItemType)
        {
            this.Id = mapItemType.Id;
            this.TerritoryId = mapItemType.TerritoryId;
            this.Name = mapItemType.Name;
            this.Description = mapItemType.Description;
        }
    }
}
