namespace AgUnit.Runner.Resharper90.MsTest14
{
    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.UnitTestProvider.MsTest;

    using JetBrains.Application;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Elements;
    using JetBrains.ReSharper.UnitTestProvider.MSTest;
    using JetBrains.ReSharper.UnitTestProvider.MSTest11;

    [SolutionComponent]
    public class SilverlightElementsSource : SilverlightElementsSourceBase
    {
        private readonly IUnitTestElementIdFactory unitTestElementIdFactory;

        public SilverlightElementsSource(
            SilverlightUnitTestProvider provider, 
            IMsTestServices msTestServices,
            IMsTestAttributesProvider msTestAttributesProvider, IUnitTestElementManager unitTestElementManager, IUnitTestElementCategoryFactory unitTestCategoryFactory, IShellLocks shellLocks, IUnitTestElementIdFactory unitTestElementIdFactory)
            : base(provider, msTestServices, msTestAttributesProvider, unitTestElementManager, unitTestCategoryFactory, shellLocks)
        {
            this.unitTestElementIdFactory = unitTestElementIdFactory;
        }

        protected override IMsTestElementFactory CreateMsTestElementFactory(
            SilverlightServices silverlightServices,
            IUnitTestElementManager unitTestElementManager,
            IUnitTestElementCategoryFactory unitTestCategoryFactory)
        {
            return new MsTestElementFactory(silverlightServices, unitTestElementManager, unitTestCategoryFactory, this.unitTestElementIdFactory);
        }
    }
}