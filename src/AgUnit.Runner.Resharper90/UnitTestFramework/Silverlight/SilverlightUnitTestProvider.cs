namespace AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform;

    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;

    [UnitTestProvider]
    public class SilverlightUnitTestProvider : IUnitTestProvider
    {
        public const string RunnerId = "Silverlight";
        public const string RunnerTypeName = "AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.SilverlightUnitTestTaskRunner";

        private static string GetRunnerCodeBase()
        {
            var pluginCodeBase = typeof(SilverlightUnitTestProvider).Assembly.CodeBase;
            var directory = Path.GetDirectoryName(pluginCodeBase);
            var filename = Path.GetFileNameWithoutExtension(pluginCodeBase) + ".TaskRunner.dll";

            return new Uri(Path.Combine(directory, filename), UriKind.RelativeOrAbsolute).LocalPath;
        }

        public string ID
        {
            get { return RunnerId; }
        }

        public string Name
        {
            get { return "Silverlight"; }
        }

        public static RemoteTaskRunnerInfo GetTaskRunnerInfo(IUnitTestLaunch launch)
        {
            var runnerType = Assembly.LoadFrom(GetRunnerCodeBase()).GetType(RunnerTypeName);

            var additionalPaths = launch.Runs.SelectMany(r => r.GetAllTasks())
                .Select(t => t.Task.GetType().Assembly).Distinct()
                .Where(a => !a.FullName.StartsWith("JetBrains"))
                .Select(a => Path.GetDirectoryName(new Uri(a.CodeBase, UriKind.RelativeOrAbsolute).LocalPath))
                .ToArray();

            return new RemoteTaskRunnerInfo(RunnerId, runnerType, additionalPaths);
        }

        public static RemoteTaskRunnerInfo GetTaskRunnerInfo()
        {
            var runnerType = Assembly.LoadFrom(GetRunnerCodeBase()).GetType(RunnerTypeName);
            return new RemoteTaskRunnerInfo(RunnerId, runnerType);
        }

        public bool IsElementOfKind(IDeclaredElement declaredElement, UnitTestElementKind elementKind)
        {
            return false;
        }

        public bool IsElementOfKind(IUnitTestElement element, UnitTestElementKind elementKind)
        {
            return false;
        }

        public bool IsSupported(IHostProvider hostProvider)
        {
            return true;
        }

        public int CompareUnitTestElements(IUnitTestElement x, IUnitTestElement y)
        {
            return 0;
        }
    }
}