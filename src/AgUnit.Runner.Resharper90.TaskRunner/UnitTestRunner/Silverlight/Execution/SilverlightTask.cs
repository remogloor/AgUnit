namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    using System;
    using System.Collections.Generic;
    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;

    public class SilverlightTask
    {
        public TaskNode Node { get; private set; }

        public SilverlightTask(TaskNode node)
        {
            Node = node;
        }

        public void Execute(Action<SilverlightTask> execute)
        {
            Node.Execute(execute, this);
        }

        private SilverlightUnitTestAssemblyTask GetTask()
        {
            return (SilverlightUnitTestAssemblyTask)Node.Task;
        }

        public bool HasXapPath()
        {
            return !string.IsNullOrWhiteSpace(GetTask().XapPath);
        }

        public IEnumerable<string> GetXapPaths()
        {
            return HasXapPath() ? new[] { GetTask().XapPath } : new string[0];
        }

        public IEnumerable<string> GetDllPaths()
        {
            return !HasXapPath() ? new[] { GetTask().DllPath } : new string[0];
        }
    }
}