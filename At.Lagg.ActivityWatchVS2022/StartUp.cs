using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022
{
    [CommandsPackage("At.Lagg.ActivityWatchVS2022", "1.0")]
    public class StartUp : Extension
    {
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            serviceCollection.AddSingleton<EventService>();
        }
    }
}
