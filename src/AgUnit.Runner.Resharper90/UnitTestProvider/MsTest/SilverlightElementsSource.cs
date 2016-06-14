namespace AgUnit.Runner.Resharper90.UnitTestProvider.MsTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

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
        private readonly IUnitTestElementCategoryFactory unitTestCategoryFactory;
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
            IUnitTestElementCategoryFactory unitTestCategoryFactory,
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

        public void ExploreProjects(
            IDictionary<IProject, FileSystemPath> projects,
            MetadataLoader loader,
            IUnitTestElementsObserver observer,
            CancellationToken cancellationToken)
        {
            var silverlightProjects = projects.Where(p => p.Key.IsSilverlight()).ToDictionary(p => p.Key, p => p.Value);

            this.metadataElementsSource.ExploreProjects(silverlightProjects, loader, observer, this.ExploreAssembly, cancellationToken);
            observer.OnCompleted();
        }

        public void ExploreFile(IFile psiFile, IUnitTestElementsObserver observer, Func<bool> interrupted)
        {
            if (psiFile.GetProject().IsSilverlight())
            {
                observer = new SLObserver(observer);
                if (!string.Equals(psiFile.Language.Name, "CSHARP", StringComparison.Ordinal) && !string.Equals(psiFile.Language.Name, "VBASIC", StringComparison.Ordinal) || psiFile.GetSourceFile().ToProjectFile() == null)
                    return;
                this.RunWithElementFactory(elementFactory => psiFile.ProcessDescendants(new MsTestFileExplorer(elementFactory, this.msTestAttributesProvider, observer, psiFile, interrupted)));
            }
        }

        private void ExploreAssembly(IProject project, IMetadataAssembly assembly, IUnitTestElementsObserver observer, CancellationToken cancellationToken)
        {
            observer = new SLObserver(observer);
            if (assembly.ReferencedAssembliesNames.Any(n => n.Name == SilverlightMsTestAssemblyName))
            {
                this.RunWithElementFactory(elementFactory => new MsTestMetadataExplorer(elementFactory, this.msTestAttributesProvider, observer, project).ExploreAssembly(assembly, cancellationToken));
            }
        }

        private void RunWithElementFactory(Action<IMsTestElementFactory> action)
        {
            var silverlightServices = new SilverlightServices(this.msTestServices, this.Provider);
            var elementFactory = this.CreateMsTestElementFactory(silverlightServices, this.unitTestElementManager, this.unitTestCategoryFactory);

            action(elementFactory);

            silverlightServices.ExplorationCompleted = true;
        }

        protected abstract IMsTestElementFactory CreateMsTestElementFactory(SilverlightServices silverlightServices, IUnitTestElementManager unitTestElementManager, IUnitTestElementCategoryFactory unitTestCategoryFactory);
    }

    public class SLObserver : IUnitTestElementsObserver
    {
        private readonly IUnitTestElementsObserver decoratedObserver;

        public SLObserver(IUnitTestElementsObserver decoratedObserver)
        {
            this.decoratedObserver = decoratedObserver;
        }

        public void OnUnitTestElement(IUnitTestElement element)
        {
            this.decoratedObserver.OnUnitTestElement(element);
        }

        public void OnUnitTestElementDisposition(UnitTestElementDisposition disposition)
        {
            this.decoratedObserver.OnUnitTestElementDisposition(disposition);
        }

        public void OnUnitTestElementChanged(IUnitTestElement element)
        {
            this.decoratedObserver.OnUnitTestElementChanged(element);
        }

        public void OnCompleted()
        {
            this.decoratedObserver.OnCompleted();
        }

        public IEnumerable<UnitTestElementDisposition> Dispositions { get; private set; }
    }
}