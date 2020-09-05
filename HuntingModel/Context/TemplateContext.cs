using HuntingModel.Model.Template;
using HuntingModel.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Context
{
    public class TemplateContext
    {
        public static TemplateModel LoadTemplate(string templateName)
        {
            string dataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var templatePath = Path.Combine(dataPath, "Template", templateName);
            var fileInfo = new FileInfo(templatePath);
            if (fileInfo.Exists == false)
            {
                return null;
            }
            var template = new TemplateModel(templatePath);
            return template;
        }
    }
}
