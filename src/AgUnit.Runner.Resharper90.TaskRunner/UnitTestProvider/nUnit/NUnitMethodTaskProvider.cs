namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.nUnit
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestRunner.nUnit;

    public class NUnitMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task is NUnitTestTask;
        }

        private NUnitTestTask GetTask(RemoteTask task)
        {
            return (NUnitTestTask)task;
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var methodTask = GetTask(task);

            return string.Format("{0}.{1}", methodTask.TypeName, methodTask.ShortName);
        }
    }
}