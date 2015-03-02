namespace AgUnit.Runner.Resharper90.MsTest12
{
    using JetBrains.Application.BuildScript.Application.Zones;
    using JetBrains.Platform.VisualStudio.JustVs12.Shell.Zones;
    using JetBrains.ReSharper.UnitTestFramework;

    [ZoneMarker]
    public class ZoneMarker : IRequire<IUnitTestingZone>, IRequire<IJustVs12Zone>
    {
    }
}