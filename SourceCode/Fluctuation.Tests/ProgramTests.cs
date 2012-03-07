using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using System;

namespace Fluctuation.Tests {
    [TestFixture]
    public class ProgramTests {
        private string tempPath;

        [TearDown]
        public void Teardown() {
            if (this.tempPath != null && Directory.Exists(this.tempPath)) {
                Directory.Delete(this.tempPath, true);
            }
        }

        [Test]
        public void FilesShouldBeMovedWhilePreservingRelativeStructureTest() {
            var templatePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", ""));
            var subDir = Path.Combine(templatePath, "the dir");
            Directory.CreateDirectory(subDir);
            var file = Path.Combine(subDir, "the file");
            File.Create(file).Close();

            var templateProvider = MockRepository.GenerateStub<ITemplateProvider>();
            templateProvider.Stub(x => x.GetTemplateDirectory()).Return(templatePath);

            var modifiers = Enumerable.Empty<IContentModifier>();

            var program = new Program(templateProvider, modifiers);

            var args = new[] { "SomeName" };

            this.tempPath = Path.GetFullPath("SomeName/");

            if (Directory.Exists(this.tempPath)) {
                Directory.Delete(this.tempPath, true);
            }

            program.Run(args);

            var newFilePath = Path.Combine(this.tempPath + "\\the dir", Path.GetFileName(file));
            Assert.IsTrue(File.Exists(newFilePath));
        }

        [Test]
        public void ContentModifiersShouldChangeNewFileContents() {
            this.tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", ""));
            Directory.CreateDirectory(this.tempPath);

            var file = Path.Combine(this.tempPath, Path.GetRandomFileName());

            var contents = "file contents";
            var modifiedContents = Guid.NewGuid().ToString();

            File.WriteAllText(file, contents);

            var templateProvider = MockRepository.GenerateStub<ITemplateProvider>();
            templateProvider.Stub(x => x.GetTemplateDirectory()).Return(this.tempPath);

            var modifier = MockRepository.GenerateMock<IContentModifier>();
            modifier.Expect(x => x.Modify(contents)).Return(modifiedContents);

            var modifiers = new[] { modifier };

            var program = new Program(templateProvider, modifiers);

            program.Run(new[] { "project_name" });

            modifier.VerifyAllExpectations();
            Assert.That(File.ReadAllText("project_name/" + Path.GetFileName(file)), Is.EqualTo(modifiedContents));
        }
    }
}
