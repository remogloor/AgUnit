#if NO_SUPPORT_DEBUG

namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using EnvDTE;

    using JetBrains.PsiFeatures.VisualStudio.Core.UnitTesting;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.Util;

    /// <summary>
    /// Replacement for the correct implementation. 
    /// </summary>
    /// <remarks>
    /// Due to missing extension points the original DebugTaskRunnerHostController can't be extended with Silverlight support.
    /// A version that can do this can easily be created by reverse engineering that class and extend the assignment of guidLaunchDebugEngine with
    /// "run.IsSilverlightRun() ? new Guid("{032f4b8c-7045-4b24-accf-d08c9da108fe}") : ". Note that you do this on your own risk because this most likely
    /// violates the copyright of R#. This is the reason why such a fil isn't published.
    /// </remarks>
    public class ExtendedDebugTaskRunnerHostController : DebugTaskRunnerHostController
    {
        public ExtendedDebugTaskRunnerHostController(ILogger logger, IUnitTestLaunchManager launchManager, IUnitTestResultManager resultManager, IUnitTestAgentManager agentManager, IVsDebuggerFacade debuggerFacade, DTE dte, IUnitTestLaunch launch, int portNumber)
            : base(logger, launchManager, resultManager, agentManager, debuggerFacade, dte, launch, portNumber)
        {
        }
    }
}

#endif