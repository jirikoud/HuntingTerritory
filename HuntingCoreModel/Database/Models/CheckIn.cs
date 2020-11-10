using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class CheckIn
    {
        public long Id { get; set; }
        public long MapItemId { get; set; }
        public long? QuestionnaireId { get; set; }
        public string Note { get; set; }
        public DateTime CheckTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SysCreated { get; set; }
        public long SysCreator { get; set; }
        public DateTime? SysUpdated { get; set; }
        public long? SysEditor { get; set; }
    }
}
