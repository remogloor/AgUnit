namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;
    using AgUnit.Runner.Resharper90.Util;

    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;

    public static class UnitTestRunExtensions
    {
        public static bool IsSilverlightRun(this IUnitTestRun run)
        {
            return run.GetSilverlightPlatformVersion() != null;
        }

        public static Version GetSilverlightPlatformVersion(this IUnitTestRun run)
        {
            var sequence = run.GetRootTasks().FirstOrDefault();
            return sequence != null ? sequence.GetSilverlightPlatformVersion() : null;
        }

        public static IList<RemoteTaskPacket> GetRootTasks(this IUnitTestRun run)
        {
            return (run.GetTasks() as IList<RemoteTaskPacket>) ?? new List<RemoteTaskPacket>();
        }

        public static IEnumerable<RemoteTaskPacket> GetAllTasks(this IUnitTestRun run)
        {
            return run.GetTasks().SelectMany(t => t.GetAllTasksRecursive());
        }

        public static void AddTaskSequence(this IUnitTestRun run, RemoteTaskPacket sequence, SilverlightUnitTestElement silverlightElement, IUnitTestRun originalRun)
        {
            var runTasks = originalRun.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");
            var runTaskIdsToElements = originalRun.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");

            run.SetField("myElementsToTasks", runTasks);
            run.SetField("myTaskIdsToElements", runTaskIdsToElements);

            run.GetRootTasks().Add(sequence);
            runTasks[silverlightElement] = sequence.Task;
            runTaskIdsToElements[sequence.Task.Id] = silverlightElement;
        }

        public static void AddTaskSequence(this IUnitTestRun run, RemoteTaskPacket sequence, IUnitTestRun originalRun)
        {
            var runTasks = run.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");
            var runTaskIdsToElements = run.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");
            var originalRunTasks = originalRun.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");
            var originalRunTaskIdsToElements = originalRun.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");

            foreach (var originalRunTask in originalRunTasks)
            {
                runTasks[originalRunTask.Key] = originalRunTask.Value;
            }

            foreach (var originalRunTaskIdsToElement in originalRunTaskIdsToElements)
            {
                runTaskIdsToElements[originalRunTaskIdsToElement.Key] = originalRunTaskIdsToElement.Value;
            }
        }
    }
}