using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.SqlGenerator.Generators
{
    public class CheckInFilter : FulltextFilterBase
    {
        public const string SORT_USER_NAME = "User";
        public const string SORT_QUESTIONNAIRE = "Questionnaire";
        public const string SORT_CHECKIN_TIME = "CheckInTime";

        [Display(Name = "FILTER_MAP_ITEM", ResourceType = typeof(CheckInRes))]
        public int? MapItemId { get; set; }

        [Display(Name = "FILTER_QUESTIONNAIRE", ResourceType = typeof(CheckInRes))]
        public int? QuestionnaireId { get; set; }

        [Display(Name = "FILTER_ACL_USER", ResourceType = typeof(CheckInRes))]
        public int? AclUserId { get; set; }

        public CheckInFilter()
        {
            this.SortField = SORT_CHECKIN_TIME;
            this.SortIsDesc = true;
        }

        public void PrepareFilter()
        {
        }
    }
}
