namespace AgUnit.Runner.Resharper90
{
    using JetBrains.Application.BuildScript.Application.Zones;
    using JetBrains.ReSharper.UnitTestFramework;

    [ZoneMarker]
    public class ZoneMarker : IRequire<IUnitTestingZone>
    {
    }
}