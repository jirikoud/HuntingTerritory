using HuntingModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.AccountModels
{
    public class LanguageListModel
    {
        public List<LanguageModel> LanguageList { get; set; }

        public LanguageListModel(List<Language> languageList)
        {
            this.LanguageList = languageList.ConvertAll(item => new LanguageModel(item));
        }
    }
}
