namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform;

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.UnitTestFramework;

    public class HostProviderWrapper : IHostProvider
    {
        public IHostProvider WrappedHostProvider { get; private set; }

        private IUnitTestElementIdFactory elementIdFactory;

        private IUnitTestProviderManager unitTestProviderManager;

        public HostProviderWrapper(IHostProvider wrappedHostProvider, IUnitTestElementIdFactory elementIdFactory, IUnitTestProviderManager unitTestProviderManager)
        {
            this.elementIdFactory = elementIdFactory;
            this.unitTestProviderManager = unitTestProviderManager;
            this.WrappedHostProvider = wrappedHostProvider;
        }

        public ITaskRunnerHostController CreateHostController(
            ISolution solution,
            IUnitTestLaunchManager launchManager,
            IUnitTestResultManager resultManager,
            IUnitTestAgentManager agentManager,
            IUnitTestLaunch launch)
        {
            var hostController = this.CreateWrappedHostController(solution, launchManager, resultManager, agentManager, launch);

            launch.EnsureSilverlightPlatformSupport(unitTestProviderManager, hostController, elementIdFactory);

            return hostController;
        }

        public string ID
        {
            get { return this.WrappedHostProvider.ID; }
        }

        public HostProviderAvailability GetAvailability()
        {
            return this.WrappedHostProvider.GetAvailability();
        }

        public HostProviderAvailability GetAvailability(IUnitTestElement element)
        {
            return this.WrappedHostProvider.GetAvailability(element);
        }

        public bool SupressBuild()
        {
            return this.WrappedHostProvider.SupressBuild();
        }

        protected virtual ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestResultManager resultManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            return this.WrappedHostProvider.CreateHostController(solution, launchManager, resultManager, agentManager, launch);
        }
    }
}   