namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgUnit.Runner.Resharper90.Util;

    using JetBrains.Application;
    using JetBrains.PsiFeatures.VisualStudio.Core.UnitTesting;
    using JetBrains.ReSharper.UnitTestFramework;

    [ShellComponent]
    public class HostProviderInterceptor
    {
        public HostProviderInterceptor()
        {
            var hostProviders = GetHostProviders();

            var providers = new List<string>();
            providers.AddRange(Wrap<ProcessHostProvider>(hostProviders, p => new HostProviderWrapper<ProcessHostProvider>(p)));
            providers.AddRange(Wrap<DebugHostProvider>(hostProviders, p => new ExtendedDebugHostProvider(p)));

            IDictionary<string, IHostProviderDescriptor> descriptors = UnitTestHost.Instance.GetProviderDescriptors().ToDictionary(d => d.Provider.ID, d => d);
            foreach (var provider in providers)
            {
                descriptors[provider] = new HostProviderDescriptorWrapper(UnitTestHost.Instance.GetProvider(provider), descriptors[provider]);
            }

            UnitTestHost.Instance.SetField("myHostProviderDescriptors", descriptors.Values.ToArray());
        }

        private static IDictionary<string, IHostProvider> GetHostProviders()
        {
            var unitTestHost = UnitTestHost.Instance;
            var hostProviders = unitTestHost.GetField<IDictionary<string, IHostProvider>>("myHostProviders");

            return hostProviders;
        }

        private static IEnumerable<string> Wrap<THostProvider>(IDictionary<string, IHostProvider> hostProviders, Func<THostProvider, HostProviderWrapper<THostProvider>> createWrapper)
            where THostProvider : IHostProvider
        {
            foreach (var hostProvider in hostProviders.ToList())
            {
                if (hostProvider.Value is THostProvider)
                {
                    hostProviders[hostProvider.Key] = createWrapper((THostProvider)hostProvider.Value);
                    yield return hostProvider.Key;
                }
            }
        }
    }
}