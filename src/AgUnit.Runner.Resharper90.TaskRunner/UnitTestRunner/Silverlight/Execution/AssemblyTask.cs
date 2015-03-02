namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;

    public class AssemblyTask
    {
        public TaskNode Node { get; private set; }
        public IAssemblyTaskProvider AssemblyTaskProvider { get; private set; }

        public AssemblyTask(TaskNode node, IAssemblyTaskProvider assemblyTaskProvider)
        {
            Node = node;
            AssemblyTaskProvider = assemblyTaskProvider;
        }

        public string GetAssemblyLocation()
        {
            return AssemblyTaskProvider.GetAssemblyLocation(Node.Task);
        }
    }
}