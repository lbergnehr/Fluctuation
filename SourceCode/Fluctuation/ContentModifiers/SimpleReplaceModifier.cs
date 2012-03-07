using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluctuation.ContentModifiers {
    public class SimpleReplaceModifier : IContentModifier {
        private string name;

        public SimpleReplaceModifier(string[] args) {
            this.name = args.LastOrDefault();
        }

        #region IContentModifier Members

        public string Modify(string content) {
            return content.Replace("__NAME__", this.name);
        }

        #endregion
    }
}
