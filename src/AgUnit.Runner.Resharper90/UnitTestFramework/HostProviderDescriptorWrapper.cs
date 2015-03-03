namespace AgUnit.Runner.Resharper90.UnitTestFramework
{
    using JetBrains.Application.Components;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Resources;
    using JetBrains.UI.Icons;

    public class HostProviderDescriptorWrapper : IHostProviderDescriptor
    {
        private readonly IHostProviderDescriptor wrappedDescriptor;

        public IHostProvider Provider { get; private set; }

        public int Priority
        {
            get
            {
                return this.wrappedDescriptor.Priority;
            }
        }

        public IconId Icon
        {
            get
            {
                return this.wrappedDescriptor.Icon;
            }
        }

        public string Format
        {
            get
            {
                return this.wrappedDescriptor.Format;
            }
        }

        public string ShortText
        {
            get
            {
                return this.wrappedDescriptor.ShortText;
            }
        }

        public HostProviderDescriptorWrapper(IHostProvider provider, IHostProviderDescriptor wrappedDescriptor)
        {
            this.Provider = provider;
            this.wrappedDescriptor = wrappedDescriptor;
        }
    }
}