namespace AgUnit.Runner.Resharper90.MsTest11
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
        private readonly IUnitTestElementIdFactory elementIdFactory;

        public SilverlightElementsSource(
            SilverlightUnitTestProvider provider, 
            IMsTestServices msTestServices,
            IMsTestAttributesProvider msTestAttributesProvider, IUnitTestElementManager unitTestElementManager, IUnitTestElementCategoryFactory unitTestCategoryFactory, IShellLocks shellLocks, IUnitTestElementIdFactory elementIdFactory)
            : base(provider, msTestServices, msTestAttributesProvider, unitTestElementManager, unitTestCategoryFactory, shellLocks)
        {
            this.elementIdFactory = elementIdFactory;
        }

        protected override IMsTestElementFactory CreateMsTestElementFactory(
            SilverlightServices silverlightServices,
            IUnitTestElementManager unitTestElementManager,
            IUnitTestElementCategoryFactory unitTestCategoryFactory)
        {
            return new MsTestElementFactory(silverlightServices, unitTestElementManager, unitTestCategoryFactory, this.elementIdFactory);
        }
    }
}