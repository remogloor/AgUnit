namespace AgUnit.Runner.Resharper90.UnitTestProvider.MsTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform;

    using JetBrains.Application;
    using JetBrains.Metadata.Reader.API;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Elements;
    using JetBrains.ReSharper.UnitTestProvider.MSTest;
    using JetBrains.Util;
    using JetBrains.Util.Logging;

    public abstract class SilverlightElementsSourceBase : IUnitTestElementsSource
    {
        private const string SilverlightMsTestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTesting.Silverlight";

        private readonly MetadataElementsSource metadataElementsSource;

        private readonly IMsTestServices msTestServices;
        private readonly IMsTestAttributesProvider msTestAttributesProvider;
        private readonly IUnitTestElementManager unitTestElementManager;
        private readonly IUnitTestCategoryFactory unitTestCategoryFactory;
        private readonly IShellLocks shellLocks;

        public IUnitTestProvider Provider
        {
            get;
            private set;
        }

        protected SilverlightElementsSourceBase(
            SilverlightUnitTestProvider provider,
            IMsTestServices msTestServices,
            IMsTestAttributesProvider msTestAttributesProvider,
            IUnitTestElementManager unitTestElementManager,
            IUnitTestCategoryFactory unitTestCategoryFactory,
            IShellLocks shellLocks)
        {
            this.Provider = provider;
            this.msTestServices = msTestServices;
            this.msTestAttributesProvider = msTestAttributesProvider;
            this.unitTestElementManager = unitTestElementManager;
            this.unitTestCategoryFactory = unitTestCategoryFactory;
            this.shellLocks = shellLocks;
            this.metadataElementsSource = new MetadataElementsSource(Logger.GetLogger(typeof(SilverlightElementsSourceBase)), shellLocks);
        }

        public void ExploreSolution(IUnitTestElementsObserver observer)
        {
        }

        public void ExploreProjects(IDictionary<IProject, FileSystemPath> projects, MetadataLoader loader, IUnitTestElementsObserver observer)
        {
            var silverlightProjects = projects.Where(p => p.Key.IsSilverlight()).ToDictionary(p => p.Key, p => p.Value);

            this.metadataElementsSource.ExploreProjects(silverlightProjects, loader, observer, this.ExploreAssembly);
            observer.OnCompleted();
        }

        public void ExploreFile(IFile psiFile, IUnitTestElementsObserver observer, Func<bool> interrupted)
        {
            if (psiFile.GetProject().IsSilverlight())
            {
                if (!string.Equals(psiFile.Language.Name, "CSHARP", StringComparison.Ordinal) && !string.Equals(psiFile.Language.Name, "VBASIC", StringComparison.Ordinal) || psiFile.GetSourceFile().ToProjectFile() == null)
                    return;
                this.RunWithElementFactory(elementFactory => psiFile.ProcessDescendants(new MsTestFileExplorer(elementFactory, this.msTestAttributesProvider, observer, psiFile, interrupted)));
            }
        }

        private void ExploreAssembly(IProject project, IMetadataAssembly assembly, IUnitTestElementsObserver observer)
        {
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                this.RunWithElementFactory(elementFactory => new MsTestMetadataExplorer(elementFactory, this.msTestAttributesProvider, project, this.shellLocks, observer).ExploreAssembly(assembly));
            }
        }

        private void RunWithElementFactory(Action<IMsTestElementFactory> action)
        {
            var silverlightServices = new SilverlightServices(this.msTestServices, this.Provider);
            var elementFactory = this.CreateMsTestElementFactory(silverlightServices, this.unitTestElementManager, this.unitTestCategoryFactory);

            action(elementFactory);

            silverlightServices.ExplorationCompleted = true;
        }

        protected abstract IMsTestElementFactory CreateMsTestElementFactory(SilverlightServices silverlightServices, IUnitTestElementManager unitTestElementManager, IUnitTestCategoryFactory unitTestCategoryFactory);
    }
}