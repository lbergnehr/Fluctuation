using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluctuation.Extensions;
using System.Configuration;

namespace Fluctuation.TemplateProviders {
    public class FileSystemTemplateProvider : ITemplateProvider {
        string templateDir;

        public FileSystemTemplateProvider(string[] args) {
            if (args != null) {
                this.templateDir = args.GetParameterValue("templateDir");
            }
        }

        #region ITemplateProvider Members

        public string GetTemplateDirectory() {
            return this.templateDir
                ?? ConfigurationManager.AppSettings["templateDirectoryPath"]
                ?? ".";
        }

        #endregion
    }
}
