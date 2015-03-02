namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    using JetBrains.ReSharper.TaskRunnerFramework;

    public interface IClassTaskProvider
    {
        bool IsClassTask(RemoteTask task);
        string GetFullClassName(RemoteTask task);
    }
}