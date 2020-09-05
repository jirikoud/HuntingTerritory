using HuntingModel.Database;
using HuntingModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.CheckInModels
{
    public class CheckInListItemModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string CheckTime { get; set; }
        public string QuestionnaireName { get; set; }

        public CheckInListItemModel(CheckInListItem checkIn, Language language)
        {
            this.Id = checkIn.Id;
            this.UserName = checkIn.UserName;
            this.CheckTime = ContextUtils.FormatDateTime(checkIn.CheckInTime, language);
            this.QuestionnaireName = checkIn.QuestionnaireName;
        }
    }
}
