namespace AgUnit.Runner.Resharper90.MsTest11
{
    using JetBrains.Application.BuildScript.Application.Zones;
    using JetBrains.Platform.VisualStudio.JustVs11.Shell.Zones;
    using JetBrains.ReSharper.UnitTestFramework;

    [ZoneMarker]
    public class ZoneMarker : IRequire<IUnitTestingZone>, IRequire<IJustVs11Zone>
    {
    }
}