﻿namespace AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using AgUnit.Runner.Resharper90.TaskRunner.UnitTestRunner.Silverlight.Execution;
    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;

    public static class TaskNodeExtensions
    {
        public static IEnumerable<SilverlightTask> GetSilverlightTasks(this TaskNode node)
        {
            if (node.Task is SilverlightUnitTestAssemblyTask)
            {
                yield return new SilverlightTask(node);
            }

            foreach (var child in node.Children.SelectMany(child => child.GetSilverlightTasks()))
            {
                yield return child;
            }
        }

        public static IEnumerable<AssemblyTask> GetAssemblyTasks(this TaskNode node)
        {
            foreach (var assemblyTaskProvider in node.Environment.AssemblyTaskProviders)
            {
                if (assemblyTaskProvider.IsAssemblyTask(node.Task))
                {
                    yield return new AssemblyTask(node, assemblyTaskProvider);
                }
            }

            foreach (var child in node.Children.SelectMany(child => child.GetAssemblyTasks()))
            {
                yield return child;
            }
        }

        public static IEnumerable<ClassTask> GetClassTasks(this TaskNode node)
        {
            foreach (var classTaskProvider in node.Environment.ClassTaskProviders)
            {
                if (classTaskProvider.IsClassTask(node.Task))
                {
                    yield return new ClassTask(node, classTaskProvider);
                }
            }

            foreach (var child in node.Children.SelectMany(child => child.GetClassTasks()))
            {
                yield return child;
            }
        }

        public static IEnumerable<MethodTask> GetMethodTasks(this TaskNode node)
        {
            foreach (var methodTaskProvider in node.Environment.MethodTaskProviders)
            {
                if (methodTaskProvider.IsMethodTask(node.Task))
                {
                    yield return new MethodTask(node, methodTaskProvider);
                }
            }

            foreach (var child in node.Children.SelectMany(child => child.GetMethodTasks()))
            {
                yield return child;
            }
        }
    }
}