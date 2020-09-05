using HuntingModel.Database;
using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.QuestionModels
{
    public class QuestionListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QuestionType { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

        public QuestionListItemModel(Question question)
        {
            this.Id = question.Id;
            this.Name = question.Name;
            this.Description = question.Description;
            this.QuestionType = QuestionTypeEnumConvertor.GetString(question.QuestionTypeEx);
        }
    }
}
