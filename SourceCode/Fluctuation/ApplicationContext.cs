using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluctuation {
    class ApplicationContext {
        private string[] args;

        public ApplicationContext(string[] args) {
            // TODO: Complete member initialization
            this.args = args;
        }

        public string Name { get; private set; }
    }
}
