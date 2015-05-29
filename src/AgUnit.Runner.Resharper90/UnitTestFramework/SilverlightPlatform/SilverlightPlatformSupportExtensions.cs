﻿namespace AgUnit.Runner.Resharper90.UnitTestFramework.SilverlightPlatform
{
    using System;
    using System.Linq;

    using AgUnit.Runner.Resharper90.UnitTestFramework.Silverlight;

    using JetBrains.ReSharper.TaskRunnerFramework;
    using JetBrains.ReSharper.UnitTestFramework;

    using NuGet;

    public static class SilverlightPlatformSupportExtensions
    {
        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, UnitTestProviders providers, ITaskRunnerHostController hostController)
        {
            var runs = launch.GetRuns();
            RemoteTaskPacket silverlightSequence = null;
            IUnitTestRun silverlightUnitTestRun = null;

            foreach (var run in runs.Values.Select(r => r.Value).ToArray())
            {
                foreach (var sequence in run.GetRootTasks().ToArray())
                {
                    if (run.Elements.Any(e => e is SilverlightUnitTestElement))
                    {
                        continue;
                    }

                    var provider = providers.GetProvider(SilverlightUnitTestProvider.RunnerId);
                    ConvertToSilverlightSequenceIfNecessary(sequence, run, ref silverlightSequence, ref silverlightUnitTestRun, launch, provider, hostController);
                }
            }

            launch.RemoveEmptyRuns();
        }

        public static void EnsureSilverlightPlatformSupport(this IUnitTestLaunch launch, ref IUnitTestRun run, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            foreach (var sequence in run.GetRootTasks().ToArray())
            {
                ConvertToSilverlightSequenceIfNecessary(sequence, ref run, launch, provider, hostController);
            }
        }

        private static void ConvertToSilverlightSequenceIfNecessary(RemoteTaskPacket sequence, ref IUnitTestRun run, IUnitTestLaunch launch, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject(run);
                if (silverlightProject != null)
                {
                    var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, provider, hostController);
                    var silverlightElement = new SilverlightUnitTestElement(new UnitTestElementId(provider, run.Elements.First().Id.PersistentProjectId, Guid.NewGuid().ToString()), silverlightRun.Key.RunStrategy);
                    var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());

                    var silverlightSequence = new RemoteTaskPacket(remoteTask);
                    var assemblyTask = new SilverlightUnitTestAssemblyTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());
                    var assemblyTaskPacket = new RemoteTaskPacket(assemblyTask);
                    silverlightSequence.TaskPackets.Add(assemblyTaskPacket);
                    assemblyTaskPacket.TaskPackets.AddRange(sequence.TaskPackets);

                    run.GetRootTasks().Remove(sequence);
                    silverlightRun.Value.AddTaskSequence(silverlightSequence, silverlightElement, run);
                    run = silverlightRun.Value;
                }
            }
        }

        private static void ConvertToSilverlightSequenceIfNecessary(RemoteTaskPacket sequence, IUnitTestRun run, ref RemoteTaskPacket silverlightSequence, ref IUnitTestRun silverlightUnitTestRun, IUnitTestLaunch launch, IUnitTestProvider provider, ITaskRunnerHostController hostController)
        {
            if (!sequence.IsSilverlightSequence())
            {
                var silverlightProject = sequence.GetSilverlightProject(run);
                if (silverlightProject != null)
                {
                    if (silverlightSequence == null)
                    {
                        var silverlightRun = launch.GetOrCreateSilverlightRun(silverlightProject.PlatformID, provider, hostController);
                        var silverlightElement = new SilverlightUnitTestElement(new UnitTestElementId(provider, run.Elements.First().Id.PersistentProjectId, Guid.NewGuid().ToString()), silverlightRun.Key.RunStrategy);
                        var remoteTask = new SilverlightUnitTestTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());

                        silverlightSequence = new RemoteTaskPacket(remoteTask);
                        silverlightRun.Value.AddTaskSequence(silverlightSequence, silverlightElement, run);
                        silverlightUnitTestRun = silverlightRun.Value;
                    }
                    else
                    {
                        silverlightUnitTestRun.AddTaskSequence(silverlightSequence, run);
                    }

                    var assemblyTask = new SilverlightUnitTestAssemblyTask(silverlightProject.PlatformID.Version, silverlightProject.GetXapPath(), silverlightProject.GetDllPath());
                    var assemblyTaskPacket = new RemoteTaskPacket(assemblyTask);
                    silverlightSequence.TaskPackets.Add(assemblyTaskPacket);
                    assemblyTaskPacket.TaskPackets.AddRange(sequence.TaskPackets);

                    run.GetRootTasks().Remove(sequence);
                }
            }
        }
    }
}