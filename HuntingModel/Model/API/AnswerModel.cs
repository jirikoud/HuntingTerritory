using HuntingModel.Database;
using HuntingModel.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.API
{
    public class AnswerModel
    {
        public string Text { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public string Answer { get; set; }
        public bool? BoolValue { get; set; }
        public double? DoubleValue { get; set; }
        public List<AnswerCheckModel> AnswerCheckList { get; set; }

        public AnswerModel(Question question)
        {
            this.Text = question.Name;
            this.QuestionType = question.QuestionTypeEx;
        }

        public AnswerModel(Question question, string answer) : 
            this(question)
        {
            this.Answer = answer;
        }

        public AnswerModel(Question question, bool? boolValue) : 
            this(question)
        {
            this.BoolValue = boolValue ?? false;
        }

        public AnswerModel(Question question, Option option) : 
            this(question)
        {
            this.Answer = (option != null ? option.Name : null);
        }

        public AnswerModel(Question question, double? doubleValue) :
            this(question)
        {
            this.DoubleValue = doubleValue;
        }

        public AnswerModel(Question question, List<Answer> answerList) :
            this(question)
        {
            this.AnswerCheckList = answerList.ConvertAll(item => new AnswerCheckModel(item));
        }


    }
}
