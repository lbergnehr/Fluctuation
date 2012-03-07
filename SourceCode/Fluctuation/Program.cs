using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninject;

namespace Fluctuation {
    public class Program {
        static void Main(string[] args) {
            var program = CreateKernel(args).Get<Program>();

            try {
                program.Run(args);
            } catch (ApplicationException appEx) {
                Console.WriteLine(appEx.Message);
                Environment.Exit(-1);
            } catch (Exception ex) {
                var exception = ex;
                while (exception != null) {
                    Console.WriteLine(exception.Message);
                    exception = ex;
                }

                Environment.Exit(-1);
            }
        }

        string templatePath;
        private IContentModifier[] modifiers;

        public Program(ITemplateProvider templateProvider, IEnumerable<IContentModifier> modifiers) {
            this.templatePath = templateProvider.GetTemplateDirectory();
            this.modifiers = modifiers.ToArray();
        }

        public void Run(string[] args) {
            if (args == null || args.Length == 0) {
                throw new ApplicationException("You must provide a name for your new project.");
            }

            var templateRoot = Path.GetFullPath(this.templatePath);
            var newRoot = Path.GetFullPath(args.Last());

            var files = Directory.GetDirectories(templateRoot, "*", SearchOption.AllDirectories)
                .SelectMany(dir => Directory.GetFiles(dir))
                .Concat(Directory.GetFiles(templateRoot))
                .Select(templateFile => new { TemplateFile = templateFile, NewFile = templateFile.Replace(templateRoot, newRoot) });

            foreach (var file in files) {
                Directory.CreateDirectory(Path.GetDirectoryName(file.NewFile));

                // Read file contents.
                var contents = File.ReadAllText(file.TemplateFile);
                var modifiedContents = contents;

                // Let the modifiers modify it.
                foreach (var modifier in modifiers) {
                    modifiedContents = modifier.Modify(modifiedContents);
                }

                // Write to the new file.
                File.WriteAllText(file.NewFile, modifiedContents);
            }
        }

        public static IKernel CreateKernel(string[] args) {
            var kernel = new StandardKernel();

            kernel.Bind<ApplicationContext>().ToMethod(x => new ApplicationContext(args));
            kernel.Bind<ITemplateProvider>().ToMethod(x => new TemplateProviders.FileSystemTemplateProvider(args));
            kernel.Bind<IContentModifier>().ToMethod(x => new ContentModifiers.SimpleReplaceModifier(args));

            return kernel;
        }
    }
}
