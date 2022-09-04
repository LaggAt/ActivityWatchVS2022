using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using System.Resources;

namespace At.Lagg.ActivityWatchVS2022
{
    public class StartUp : Extension
    {
        #region Properties

        protected override ResourceManager? ResourceManager => Strings.ResourceManager;

        #endregion Properties

        #region Methods

        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // scope refers to a single extension part
            serviceCollection.AddScoped<ConsoleService>();
            serviceCollection.AddScoped<EventService>();
            serviceCollection.AddScoped<SolutionInfoService>();
        }

        #endregion Methods
    }
}
