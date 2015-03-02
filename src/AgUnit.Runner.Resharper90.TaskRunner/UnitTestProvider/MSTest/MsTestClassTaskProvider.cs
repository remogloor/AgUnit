namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.MSTest
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using AgUnit.Runner.Resharper90.Util;
    using JetBrains.ReSharper.TaskRunnerFramework;

    public class MsTestClassTaskProvider : IClassTaskProvider
    {
        public bool IsClassTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestClassTask";
        }

        public string GetFullClassName(RemoteTask task)
        {
            return task.GetProperty<string>("TypeName");
        }
    }
}