namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.Util;

    using JetBrains.Application;
    using JetBrains.ProjectModel;
    using JetBrains.PsiFeatures.VisualStudio.Core.UnitTesting;
    using JetBrains.ReSharper.UnitTestFramework;

    [SolutionComponent]
    public class HostProviderInterceptor
    {
        public HostProviderInterceptor(IUnitTestProviderManager unitTestProviderManager, IUnitTestElementIdFactory elementIdFactory)
        {
            var hostProviders = GetHostProviders();

            var providers = new List<string>();
            providers.AddRange(Wrap(hostProviders, p => new HostProviderWrapper(p, elementIdFactory, unitTestProviderManager), p => p is ProcessHostProvider));
            providers.AddRange(Wrap(hostProviders, p => new ExtendedDebugHostProvider(p, elementIdFactory, unitTestProviderManager), p => p is DebugHostProvider));
            providers.AddRange(Wrap(hostProviders, p => new HostProviderWrapper(p, elementIdFactory, unitTestProviderManager), p => p.GetType().Name == "CoverageHostProvider"));

            IDictionary<string, IHostProviderDescriptor> descriptors = UnitTestHost.Instance.GetProviderDescriptors().ToDictionary(d => d.Provider.ID, d => d);
            foreach (var provider in providers)
            {
                descriptors[provider] = new HostProviderDescriptorWrapper(UnitTestHost.Instance.GetProvider(provider), descriptors[provider]);
            }

            UnitTestHost.Instance.SetField("myHostProviderDescriptors", Enumerable.ToArray(descriptors.Values));
        }

        private static IDictionary<string, IHostProvider> GetHostProviders()
        {
            var unitTestHost = UnitTestHost.Instance;
            var hostProviders = unitTestHost.GetField<IDictionary<string, IHostProvider>>("myHostProviders");

            return hostProviders;
        }

        private static IEnumerable<string> Wrap(IDictionary<string, IHostProvider> hostProviders, Func<IHostProvider, HostProviderWrapper> createWrapper, Predicate<IHostProvider> doWrap)
        {
            foreach (var hostProvider in hostProviders.ToList())
            {
                if (doWrap(hostProvider.Value))
                {
                    hostProviders[hostProvider.Key] = createWrapper(hostProvider.Value);
                    yield return hostProvider.Key;
                }
            }
        }
    }
}