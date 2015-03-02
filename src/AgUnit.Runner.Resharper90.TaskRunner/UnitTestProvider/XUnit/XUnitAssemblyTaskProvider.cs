namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.XUnit
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using AgUnit.Runner.Resharper90.Util;
    using JetBrains.ReSharper.TaskRunnerFramework;

    public class XUnitAssemblyTaskProvider : IAssemblyTaskProvider
    {
        public bool IsAssemblyTask(RemoteTask task)
        {
            return task.GetType().FullName == "XunitContrib.Runner.ReSharper.RemoteRunner.XunitTestAssemblyTask";
        }

        public string GetAssemblyLocation(RemoteTask task)
        {
            return task.GetProperty<string>("AssemblyLocation");
        }
    }
}