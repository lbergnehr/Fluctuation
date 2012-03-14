using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluctuation.ContentModifiers {
    public class NewGuidModifier : IContentModifier {
        public string Modify(string content) {
            return content.Replace("__GUID__", Guid.NewGuid().ToString());
        }
    }
}
