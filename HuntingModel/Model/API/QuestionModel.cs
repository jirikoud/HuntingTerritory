using HuntingModel.Database;
using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }

        public List<OptionModel> OptionList { get; set; } 

        public QuestionModel(Question question)
        {
            this.Id = question.Id;
            this.Name = question.Name;
            this.Description = question.Description;
            this.Order = question.Order;
            this.QuestionType = question.QuestionTypeEx;

            if (question.QuestionTypeEx == QuestionTypeEnum.CheckboxList || question.QuestionTypeEx == QuestionTypeEnum.DropDown)
            {
                var optionList = question.Options.Where(item => item.IsDeleted == false).ToList();
                this.OptionList = optionList.ConvertAll(item => new OptionModel(item));
            }

        }
    }
}
