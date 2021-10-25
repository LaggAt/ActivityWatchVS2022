using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Helpers;
using Microsoft.VisualStudio.LocalLogger;
using Microsoft.VisualStudio.Settings.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022.Services
{
    public class EventService : DisposableObject
    {
        private readonly ExtensionCore _container;
        private readonly VisualStudioExtensibility _extensibility;
        private readonly object _initializationTask;
        private readonly ConsoleService _consoleService;

        public EventService(ExtensionCore container, VisualStudioExtensibility extensibility, ConsoleService consoleService)
            : base()
        {
            this._container = Requires.NotNull(container, nameof(container));
            this._extensibility = Requires.NotNull(extensibility, nameof(extensibility));
            this._consoleService = Requires.NotNull(consoleService, nameof(consoleService));
        }

        public async Task AddEvent(Microsoft.VisualStudio.RpcContracts.Editor.TextView textView, [CallerMemberName] string? caller = null)
        {
            string? document = textView.Document?.Uri?.AbsolutePath;



        }

        protected override void DisposeManagedResources()
        {
            finishQueue();
            base.DisposeManagedResources();
        }

        private void finishQueue()
        {
            //TODO
        }
    }
}
