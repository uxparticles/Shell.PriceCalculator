using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Shell.PriceCalculator.Engine.Tests.Unit")]
[assembly: InternalsVisibleTo("Shell.PriceCalculator")]

// this is necessary to allow the Moq framework to create objects on the fly
// ideally in production should not be enabled. Wrap in conditional compile constants #IF DEBUG.... #ENDIF
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]