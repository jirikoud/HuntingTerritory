using HtmlAgilityPack;
using HuntingModel.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntingModel.Model.Template
{
    public class TemplateModel
    {
        private HtmlDocument templateDocument;

        public string Title { get; set; }

        public string TemplateContent
        {
            get
            {
                return templateDocument.DocumentNode.OuterHtml;
            }
        }

        #region Private Methods

        private void ProcessTitle(HtmlNode documentNode)
        {
            var node = documentNode.SelectSingleNode("//title");
            if (node != null && string.IsNullOrWhiteSpace(node.InnerText) == false)
            {
                this.Title = node.InnerText;
                return;
            }
            this.Title = GlobalRes.EMAIL_SUBJECT_DEFAULT;
        }

        #endregion

        public TemplateModel(string templatePath)
        {
            var fileContent = File.ReadAllText(templatePath);
            templateDocument = new HtmlDocument();
            templateDocument.LoadHtml(fileContent);
            ProcessTitle(templateDocument.DocumentNode);
        }
    }
}
