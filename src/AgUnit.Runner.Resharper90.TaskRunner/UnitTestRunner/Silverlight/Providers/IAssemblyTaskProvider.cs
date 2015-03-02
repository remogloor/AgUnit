namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    using JetBrains.ReSharper.TaskRunnerFramework;

    public interface IAssemblyTaskProvider
    {
        bool IsAssemblyTask(RemoteTask task);
        string GetAssemblyLocation(RemoteTask task);
    }
}