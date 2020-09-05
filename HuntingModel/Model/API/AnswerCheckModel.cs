using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class AnswerCheckModel
    {
        public bool BoolValue { get; set; }
        public string Name { get; set; }

        public AnswerCheckModel(Answer answer)
        {
            this.BoolValue = answer.BoolValue ?? false;
            this.Name = answer.Option != null ? answer.Option.Name : null;
        }
    }
}
