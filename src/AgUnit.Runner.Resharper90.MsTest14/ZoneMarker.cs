namespace AgUnit.Runner.Resharper90.MsTest14
{
    using JetBrains.Application.BuildScript.Application.Zones;
    using JetBrains.Platform.VisualStudio.JustVs14.Shell.Zones;
    using JetBrains.ReSharper.UnitTestFramework;

    [ZoneMarker]
    public class ZoneMarker : IRequire<IUnitTestingZone>, IRequire<IJustVs14Zone>
    {
    }
}