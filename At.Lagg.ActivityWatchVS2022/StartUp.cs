using At.Lagg.ActivityWatchVS2022.Services;
using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

namespace At.Lagg.ActivityWatchVS2022
{
    //[CommandsPackage("At.Lagg.ActivityWatchVS2022", "1.0")]
    public class StartUp : Extension
    {
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            serviceCollection.AddSingleton<ConsoleService>();
            serviceCollection.AddSingleton<EventService>();
            serviceCollection.AddSingleton<SolutionInfoService>();
        }
    }
}
