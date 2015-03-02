namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestProvider.XUnit
{
    using System;
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers;
    using AgUnit.Runner.Resharper90.Util;
    using JetBrains.ReSharper.TaskRunnerFramework;

    public class XUnitMethodTaskProvider : IMethodTaskProvider
    {
        public bool IsMethodTask(RemoteTask task)
        {
            return task.GetType().FullName == "XunitContrib.Runner.ReSharper.RemoteRunner.XunitTestMethodTask";
        }

        public string GetFullMethodName(RemoteTask task)
        {
            var typeName = task.GetProperty<string>("TypeName");
            
            string methodName;
            
            try
            {
                methodName = task.GetProperty<string>("MethodName");
            }
            catch (Exception)
            {
                methodName = task.GetProperty<string>("ShortName");
            }

            return string.Format("{0}.{1}", typeName, methodName);
        }
    }
}