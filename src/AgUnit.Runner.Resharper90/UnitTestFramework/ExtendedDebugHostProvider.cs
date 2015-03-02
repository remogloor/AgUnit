namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using AgUnit.Runner.Resharper90.Util;

    using EnvDTE;

    using JetBrains.ProjectModel;
    using JetBrains.PsiFeatures.VisualStudio.Core.UnitTesting;
    using JetBrains.ReSharper.UnitTestExplorer;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.Threading;
    using JetBrains.Util;

    public class ExtendedDebugHostProvider : HostProviderWrapper<DebugHostProvider>
    {
        private readonly IVsDebuggerFacade debugger2;
        private readonly DTE dte;
        private readonly IThreading threading;
        private readonly ILogger logger;

        public ExtendedDebugHostProvider(DebugHostProvider wrappedHostProvider)
            : base(wrappedHostProvider)
        {
            this.debugger2 = wrappedHostProvider.GetField<IVsDebuggerFacade>("myDebuggerFacade");
            this.dte = wrappedHostProvider.GetField<DTE>("myDte");
            this.logger = wrappedHostProvider.GetField<ILogger>("myLogger");
        }

        protected override ITaskRunnerHostController CreateWrappedHostController(ISolution solution, IUnitTestLaunchManager launchManager, IUnitTestResultManager resultManager, IUnitTestAgentManager agentManager, IUnitTestLaunch launch)
        {
            return new ExtendedDebugTaskRunnerHostController(this.logger, launchManager, resultManager, agentManager, this.debugger2, this.dte, launch, solution.GetComponent<UnitTestServer>().PortNumber);
        }
    }
}