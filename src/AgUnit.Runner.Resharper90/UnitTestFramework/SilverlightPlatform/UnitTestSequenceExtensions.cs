namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;

    using JetBrains.Application.platforms;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;

    public static class UnitTestSequenceExtensions
    {
        public static IEnumerable<RemoteTaskPacket> GetAllTasksRecursive(this RemoteTaskPacket task)
        {
            yield return task;

            foreach (var childTask in task.TaskPackets.GetAllTasksRecursive())
            {
                yield return childTask;
            }
        }

        public static IEnumerable<RemoteTaskPacket> GetAllTasksRecursive(this IEnumerable<RemoteTaskPacket> tasks)
        {
            return tasks.SelectMany(GetAllTasksRecursive);
        }

        public static IProject GetSilverlightProject(this RemoteTaskPacket sequence, IUnitTestRun run)
        {
            return sequence.GetAllTasksRecursive()
                .Select(task => run.GetElementByRemoteTaskId(task.Task.Id))
                .Where(element => element != null)
                .Select(element => element.Id.GetProject())
                .FirstOrDefault(project => project != null && project.PlatformID != null && project.PlatformID.Identifier == FrameworkIdentifier.Silverlight);
        }

        public static bool IsSilverlightSequence(this RemoteTaskPacket sequence)
        {
            return sequence.GetSilverlightUnitTestTask() != null;
        }

        public static Version GetSilverlightPlatformVersion(this RemoteTaskPacket sequence)
        {
            var silverlightUnitTestTask = sequence.GetSilverlightUnitTestTask();
            return silverlightUnitTestTask != null ? silverlightUnitTestTask.SilverlightPlatformVersion : null;
        }

        public static SilverlightUnitTestTask GetSilverlightUnitTestTask(this RemoteTaskPacket sequence)
        {
            return sequence.Task as SilverlightUnitTestTask;
        }
    }
}