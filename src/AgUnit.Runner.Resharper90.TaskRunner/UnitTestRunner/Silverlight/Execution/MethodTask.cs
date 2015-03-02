namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Execution
{
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;

    public class MethodTask
    {
        public TaskNode Node { get; private set; }
        public IMethodTaskProvider MethodTaskProvider { get; private set; }

        public MethodTask(TaskNode node, IMethodTaskProvider methodTaskProvider)
        {
            Node = node;
            MethodTaskProvider = methodTaskProvider;
        }

        public string GetFullMethodName()
        {
            return MethodTaskProvider.GetFullMethodName(Node.Task);
        }
    }
}