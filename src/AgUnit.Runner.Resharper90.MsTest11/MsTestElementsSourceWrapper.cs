namespace AgUnit.Runner.Resharper90.MsTest11
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform;

    using JetBrains.Application;
    using JetBrains.Application.Components;
    using JetBrains.Metadata.Reader.API;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestProvider.MSTest;
    using JetBrains.ReSharper.UnitTestProvider.MSTest11;
    using JetBrains.Util;

    using MsTestElementsSource = JetBrains.ReSharper.UnitTestProvider.MSTest11.MsTestElementsSource;
    using MsTestProvider = JetBrains.ReSharper.UnitTestProvider.MSTest11.MsTestProvider;

    [SolutionComponent]
    public class MsTestElementsSourceWrapper : IUnitTestElementsSource, IHideImplementation<MsTestElementsSource>
    {
        private readonly MsTestElementsSource wrappedElementsSource;

        public IUnitTestProvider Provider
        {
            get;
            private set;
        }

        public MsTestElementsSourceWrapper(MsTestProvider provider, IMsTestElementFactory factory, IMsTestAttributesProvider attributesProvider, IShellLocks shellLocks, MsTestVSFacade msTestVsFacade)
        {
            this.Provider = provider;
            this.wrappedElementsSource = new MsTestElementsSource(provider, factory, attributesProvider, msTestVsFacade);
        }

        public void ExploreSolution(IUnitTestElementsObserver observer)
        {
            this.wrappedElementsSource.ExploreSolution(observer);
        }

        public void ExploreProjects(IDictionary<IProject, FileSystemPath> projects, MetadataLoader loader, IUnitTestElementsObserver observer)
        {
            var noneSilverlightProjects = projects.Where(p => !p.Key.IsSilverlight()).ToDictionary(p => p.Key, p => p.Value);
            this.wrappedElementsSource.ExploreProjects(noneSilverlightProjects, loader, observer);
        }

        public void ExploreFile(IFile psiFile, IUnitTestElementsObserver observer, Func<bool> interrupted)
        {
            if (!psiFile.GetProject().IsSilverlight())
            {
                this.wrappedElementsSource.ExploreFile(psiFile, observer, interrupted);
            }
        }
    }
}
