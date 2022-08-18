using At.Lagg.ActivityWatchVS2022.Services;
using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Definitions;
using System.Diagnostics;

namespace At.Lagg.ActivityWatchVS2022
{
    //[CommandsPackage("At.Lagg.ActivityWatchVS2022", "1.0")]
    public class StartUp : Extension
    {
        protected override ResourceManager? ResourceManager => base.ResourceManager;

        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            //Microsoft.VisualStudio.Extensibility.VisualStudioExtensibility

            serviceCollection.AddSingleton<ConsoleService>();
            serviceCollection.AddSingleton<EventService>();
            serviceCollection.AddSingleton<SolutionInfoService>();
        }
    }
}
