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
            var runTasks = run.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");
            var runTaskIdsToElements = run.GetField<Dictionary<string, IUnitTestElement>>("myTaskIdsToElements");
            var runElementsToTasks = run.GetField<Dictionary<IUnitTestElement, RemoteTask>>("myElementsToTasks");

            if (runTasks == null)
            {
                runTasks = new Dictionary<IUnitTestElement, RemoteTask>();
                run.SetField("myElementsToTasks", runTasks);
            }

            if (runTaskIdsToElements == null)
            {
                runTaskIdsToElements = new Dictionary<string, IUnitTestElement>();
                run.SetField("myTaskIdsToElements", runTaskIdsToElements);
            }

            if (runElementsToTasks == null)
            {
                runElementsToTasks = new Dictionary<IUnitTestElement, RemoteTask>();
                run.SetField("myElementsToTasks", runElementsToTasks);
            }

            foreach (var unitTestTask in sequence.GetAllTasksRecursive())
            {
                var element = originalRun.GetElementByRemoteTaskId(unitTestTask.Task.Id);

                if (element != null)
                {
                    runTasks[element] = unitTestTask.Task;
                    runTaskIdsToElements[unitTestTask.Task.Id] = element;
                    runElementsToTasks[element] = unitTestTask.Task;
                }
            }

            run.GetRootTasks().Add(sequence);
            runTasks[silverlightElement] = sequence.Task;
            runTaskIdsToElements[sequence.Task.Id] = silverlightElement;
            runElementsToTasks[silverlightElement] = sequence.Task;
        }
    }
}