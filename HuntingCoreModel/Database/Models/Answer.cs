using System;
using System.Collections.Generic;
using System.Text;

namespace HuntingCoreModel.Database.Models
{
    public class Answer
    {
        public long Id { get; set; }
        public string StringValue { get; set; }
        public float? FloatValue { get; set; }
        public int? IntValue { get; set; }
        public bool? BoolValue { get; set; }
        public long? OptionId { get; set; }
        public long CheckInId { get; set; }
        public long QuestionId { get; set; }

        public Option Option { get; set; }
    }
}
