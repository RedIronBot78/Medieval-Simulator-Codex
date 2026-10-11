using System.Reflection;

namespace Simulation.Tests;

public class SetupSmokeTests
{
    [Fact]
    public void CoreAssemblyLoadsWithoutGodotDependencies()
    {
        var core = Assembly.Load("Simulation.Core");

        Assert.Equal("Simulation.Core", core.GetName().Name);
        Assert.DoesNotContain(core.GetReferencedAssemblies(),
            reference => reference.Name?.Contains("Godot", StringComparison.OrdinalIgnoreCase) == true);
    }
}
