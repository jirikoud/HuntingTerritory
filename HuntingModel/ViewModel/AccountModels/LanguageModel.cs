using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class LanguageModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public LanguageModel(Language language)
        {
            this.Id = language.Id;
            this.Code = language.Code;
            this.Name = language.Name;
        }
    }
}
