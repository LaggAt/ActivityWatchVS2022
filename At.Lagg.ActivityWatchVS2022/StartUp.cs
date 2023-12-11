using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace At.Lagg.ActivityWatchVS2022
{
    //[CommandsPackage("At.Lagg.ActivityWatchVS2022", "1.0")]
    [VisualStudioContribution]
    public class StartUp : Extension
    {
        //protected override ResourceManager? ResurceManager => base.ResourceManager;

        #region Methods

        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                id: "ActivityWatchVS2022.7242F2A5-CE54-4A8D-8990-FF070E5AF6B1",
                version: this.ExtensionAssemblyVersion,
                publisherName: "Florian Lagg, github.com/LaggAt",
                displayName: "ActivityWatch VS 2022",
                description:
                    """
                    Track your work with this plugin and activitywatch.net. Ever wanted to know where you spend your time?
                    The Plugin is a Watcher for Visual Studio 2022. It enables tracking of all you do in your solution. We send this data to an Activity Watch installation on your machine, all tracked data belongs to you.

                    Activity Watch tracks windows titles, we extend this functionality from inside Visual Studio and track your file edits.
                    """
                ),
        };

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