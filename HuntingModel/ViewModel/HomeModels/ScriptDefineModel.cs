using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.ViewModel.HomeModels
{
    public class ScriptDefineModel
    {
        public string Lang { get; set; }
        public string Locale { get; set; }
        public bool IsDebug { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string MapAPIKey { get; set; }
    }
}
