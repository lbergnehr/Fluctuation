using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Fluctuation.TemplateProviders;

namespace Fluctuation.Tests {
    [TestFixture]
    public class FileSystemTemplateProviderTests {
        [Test]
        public void ShouldParseTemplateDirFromArgsTest() {
            var args = new[] { "-something", "-templateDir", "test", "something else" };

            var provider = new FileSystemTemplateProvider(args);
            Assert.That(provider.GetTemplateDirectory(), Is.EqualTo("test"));
        }
    }
}
