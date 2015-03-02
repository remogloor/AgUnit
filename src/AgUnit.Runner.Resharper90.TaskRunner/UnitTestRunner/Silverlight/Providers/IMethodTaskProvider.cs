namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    using JetBrains.ReSharper.TaskRunnerFramework;

    public interface IMethodTaskProvider
    {
        bool IsMethodTask(RemoteTask task);
        string GetFullMethodName(RemoteTask task);
    }
}