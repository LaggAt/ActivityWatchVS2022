using Extensibility;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Helpers;
using Microsoft.VisualStudio.LocalLogger;
using Microsoft.VisualStudio.Settings.Internal;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class EventService : ExtensionPart
    {
        private readonly ConsoleService _consoleService;
        private readonly IServiceBroker _serviceBroker;

        public EventService(ExtensionCore container, VisualStudioExtensibility extensibility, ConsoleService consoleService, IServiceBroker serviceBroker)
            : base(container, extensibility)
        {
            this._consoleService = Requires.NotNull(consoleService, nameof(consoleService));
            this._serviceBroker = Requires.NotNull(serviceBroker, nameof(serviceBroker));
        }


        public async Task AddEvent(Microsoft.VisualStudio.RpcContracts.Editor.TextView textView, [CallerMemberName] string? caller = null)
        {
            string? document = textView.Document?.Uri?.AbsolutePath;

            //TODO: how to get DTE2?
            //var x = await _serviceBroker.GetProxyAsync<DTE2>(...);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                finishQueue();
            }
            base.Dispose(isDisposing);
        }

        private void finishQueue()
        {
            //TODO
        }
    }
}
