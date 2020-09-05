using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Database
{
    partial class Question
    {
        public QuestionTypeEnum QuestionTypeEx
        {
            get
            {
                return (QuestionTypeEnum)this.QuestionType;
            }
            set
            {
                this.QuestionType = (int)value;
            }
        }
    }
}
