namespace AgUnit.Runner.Resharper90.UnitTestProvider.MsTest
{
    using System;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform;

    using JetBrains.DataFlow;
    using JetBrains.Metadata.Access;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Strategy;

    public class SilverlightRunStrategy : IUnitTestRunStrategy
    {
        private IUnitTestRunStrategy strategy;
        private readonly IUnitTestProvider provider;

        public SilverlightRunStrategy(IUnitTestProvider provider)
        {
            this.provider = provider;
        }

        public RuntimeEnvironment GetRuntimeEnvironment(
            IUnitTestElement element,
            RuntimeEnvironment projectRuntimeEnvironment,
            TargetPlatform targetPlatform,
            IUnitTestLaunch launch)
        {
            return new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };
        }

        public void Run(Lifetime lifetime, ITaskRunnerHostController runController, IUnitTestRun run, IUnitTestLaunch launch)
        {
            launch.EnsureSilverlightPlatformSupport(ref run, this.provider, runController);

            this.strategy = new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo(launch));
            this.strategy.Run(lifetime, runController, run, launch);
        }

        public void Cancel(ITaskRunnerHostController runController, IUnitTestRun run)
        {
            this.strategy.Cancel(runController, run);
        }

        public void Abort(ITaskRunnerHostController runController, IUnitTestRun run)
        {
            this.strategy.Abort(runController, run);
        }

        public bool RequiresProjectBuild(IProject project)
        {
            return true;
        }

        public bool RequiresProjectExplorationAfterBuild(IProject project, IUnitTestElement element)
        {
            return false;
        }

        public bool RequiresSeparateRunPerProject(IUnitTestElement element, IProject project)
        {
            return true;
        }
    }
}