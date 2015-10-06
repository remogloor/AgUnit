namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.Util;

    using JetBrains.Application.platforms;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestExplorer.Launch;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Strategy;

    public static class UnitTestLaunchExtensions
    {
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Select(run => run.Value).Where(run => !run.GetRootTasks().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.ID);
            }
        }

        public static KeyValuePair<UnitTestRunProperties, UnitTestRun> GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            var runs = launch.GetRuns();
            var silverlightRun = runs.Values.FirstOrDefault(run => run.Value.GetSilverlightPlatformVersion() == silverlightPlatform.Version);

            if (silverlightRun.Value == null)
            {
                var runtimeEnvironment = new RuntimeEnvironment { PlatformType = PlatformType.x86, PlatformVersion = PlatformVersion.v4_0 };

                var run = new UnitTestRun((UnitTestLaunch)launch, provider, runtimeEnvironment, runs.First().Value.Value.GetField<ISolution>("mySolution"));
                var runStrategy = new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo(launch));

                var runProperties = new UnitTestRunProperties(provider, null, runStrategy, runtimeEnvironment);
                runProperties.RunController = hostController;

                silverlightRun = new KeyValuePair<UnitTestRunProperties, UnitTestRun>(runProperties, run);

                runs.Add(run.ID, silverlightRun);
            }

            return silverlightRun;
        }

        public static Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>> GetRuns(this IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, KeyValuePair<UnitTestRunProperties, UnitTestRun>>>("myRuns");
        }
    }
}