namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.MSTest
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using AgUnit.Runner.Resharper90.Util;
    using JetBrains.ReSharper.TaskRunnerFramework;
    
    public class MsTestAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task.GetType().Name == "MsTestTestAssemblyTask";
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            return task.GetProperty<string>("AssemblyLocation");
        }
    }
}