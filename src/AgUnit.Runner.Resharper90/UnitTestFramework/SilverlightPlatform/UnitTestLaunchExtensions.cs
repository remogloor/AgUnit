namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.Util;

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestExplorer.Launch;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Strategy;

    using PlatformID = JetBrains.Application.platforms.PlatformID;

    public static class UnitTestLaunchExtensions
    {
        public static void RemoveEmptyRuns(this IUnitTestLaunch launch)
        {
            var runs = launch.GetRuns();
            var emptyRuns = runs.Values.Where(run => !run.GetRootTasks().Any()).ToArray();

            foreach (var run in emptyRuns)
            {
                runs.Remove(run.Id);
            }
        }

        public static UnitTestRun GetOrCreateSilverlightRun(this IUnitTestLaunch launch, PlatformID silverlightPlatform, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            try
            {
                var runs = launch.GetRuns();
                var silverlightRun =
                    runs.Values.FirstOrDefault(
                        run => run.GetSilverlightPlatformVersion() == silverlightPlatform.Version);

                if (silverlightRun == null)
                {
                    var runtimeEnvironment = new RuntimeEnvironment
                                                 {
                                                     PlatformType = PlatformType.x86,
                                                     PlatformVersion = PlatformVersion.v4_0
                                                 };

                    var runStrategy =
                        new OutOfProcessUnitTestRunStrategy(SilverlightUnitTestProvider.GetTaskRunnerInfo(launch));
                    var project = runs.FirstOrDefault().Value.GetProperty<UnitTestRunProperties>("Properties").Project;
                    var runProperties = new UnitTestRunProperties(provider, runStrategy, project, runtimeEnvironment);
                    runProperties.EnsureLifetime(launch);
                    silverlightRun = new UnitTestRun(runProperties, (UnitTestLaunch)launch);
                    silverlightRun.HostController = hostController;

                    runs.Add(silverlightRun.Id, silverlightRun);
                }

                return silverlightRun;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static Dictionary<string, UnitTestRun> GetRuns(this IUnitTestLaunch launch)
        {
            return launch.GetField<Dictionary<string, UnitTestRun>>("myRuns");
        }
    }
}