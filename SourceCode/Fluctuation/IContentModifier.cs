using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluctuation {
    public interface IContentModifier {
        string Modify(string content);
    }
}
