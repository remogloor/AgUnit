namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.MSTest
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using AgUnit.Runner.Resharper90.Util;
    using JetBrains.ReSharper.TaskRunnerFramework;

    public class MsTestMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestMethodTask";
        }

        public string GetFullMethodName(RemoteTask task)
        {
            return string.Format("{0}.{1}", task.GetProperty<string>("TypeName"), task.GetProperty<string>("ShortName"));
        }
    }
}