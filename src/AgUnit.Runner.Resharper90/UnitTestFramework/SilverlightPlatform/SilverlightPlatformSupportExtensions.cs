namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;

    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;

    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            var runs = launch.GetRuns();

            foreach (var run in runs.Values.Select(r => r.Value).ToArray())
            {
                foreach (var sequence in run.GetRootTasks().ToArray())
                {
                    if (run.Elements.FirstOrDefault() is SilverlightUnitTestElement)
                    {
                        continue;
                    }

                    IUnitTestRun localRun = run;
                    var provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
                    ConvertToSilverlightSequenceIfNecessary(sequence, ref localRun, launch, provider, hostController);
                }
            }

            launch.RemoveEmptyRuns();
        }

        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, ref IUnitTestRun run, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            foreach (var sequence in run.GetRootTasks().ToArray())
            {
                ConvertToSilverlightSequenceIfNecessary(sequence, ref run, launch, provider, hostController);
            }
        }

        private static void ConvertToSilverlightSequenceIfNecessary(RemoteTaskPacket sequence, ref IUnitTestRun run, IUnitTestLaunch launch, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject(run);
                if (silverlightProject != null)
                {
                    var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, provider, hostController);

                    var silverlightElement = new SilverlightUnitTestElement(new UnitTestElementId(provider, run.Elements.First().Id.PersistentProjectId, Guid.NewGuid().ToString()), silverlightRun.Key.RunStrategy);

                    var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());

                    var silverlightSequence = new RemoteTaskPacket(remoteTask) { TaskPackets = { sequence } };

                    run.GetRootTasks().Remove(sequence);
                    silverlightRun.Value.AddTaskSequence(silverlightSequence, silverlightElement, run);
                    run = silverlightRun.Value;
                }
            }
        }
    }
}