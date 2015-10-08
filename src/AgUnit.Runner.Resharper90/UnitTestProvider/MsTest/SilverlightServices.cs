﻿namespace AgUnit.Runner.Resharper90.UnitTestProvider.MsTest
{
    using JetBrains.Application.Settings;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.UnitTestFramework;
    using JetBrains.ReSharper.UnitTestFramework.Strategy;
    using JetBrains.ReSharper.UnitTestProvider.MSTest;

    public class SilverlightServices : IMsTestServices
    {
        private readonly IUnitTestProvider silverlightUnitTestProvider;
        private readonly IMsTestServices wrappedServices;
        private readonly SilverlightRunStrategy runStrategy;
        private readonly IUnitTestElementIdFactory unitTestElementIdFactory;

        public SilverlightServices(IMsTestServices wrappedServices, IUnitTestProvider silverlightUnitTestProvider)
        {
            this.silverlightUnitTestProvider = silverlightUnitTestProvider;
            this.wrappedServices = wrappedServices;
            this.runStrategy = new SilverlightRunStrategy(this.silverlightUnitTestProvider, unitTestElementIdFactory);
        }

        public IUnitTestRunStrategy GetRunStrategy(IHostProvider hostProvider, IUnitTestElement element)
        {
            return this.runStrategy;
        }

        public string GetRunConfigurationFilename(IUnitTestLaunch launch)
        {
            return this.wrappedServices.GetRunConfigurationFilename(launch);
        }

        public bool IsLegacyRunConfiguration(string runConfigurationFilename)
        {
            return this.wrappedServices.IsLegacyRunConfiguration(runConfigurationFilename);
        }

        public IUnitTestProvider Provider
        {
            get
            {
                return this.ExplorationCompleted ? this.wrappedServices.Provider : this.silverlightUnitTestProvider;
            }
        }

        public int VsVersionMajor
        {
            get
            {
                return this.wrappedServices.VsVersionMajor;
            }
        }

        public ISolution Solution
        {
            get
            {
                return this.wrappedServices.Solution;
            }
        }

        public IContextBoundSettingsStore BoundSettingsStore
        {
            get
            {
                return this.wrappedServices.BoundSettingsStore;
            }
        }

        public IUnitTestCategoryFactory CategoryFactory
        {
            get
            {
                return this.wrappedServices.CategoryFactory;
            }
        }

        public UnitTestingCachingService CachingService
        {
            get
            {
                return this.wrappedServices.CachingService;                
            }
        }

        public bool ExplorationCompleted { get; set; }
    }
}