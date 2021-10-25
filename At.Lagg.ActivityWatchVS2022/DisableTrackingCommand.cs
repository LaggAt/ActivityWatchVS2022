using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Definitions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace At.Lagg.ActivityWatchVS2022
{
    [Command(NAME, ID, "AW: disable tracking", placement: KnownCommandPlacement.ExtensionsMenu)]
    [CommandIcon("AddItem", IconSettings.IconOnly)]
    public class DisableTrackingCommand : Command
    {
        public const ushort ID = 1;
        public const string NAME = "At.Lagg.ActivityWatchVS2022.DisableTrackingCommand";

        private readonly object _syncLock = new object();
        private bool enabled = true;

        public DisableTrackingCommand(VisualStudioExtensibility extensibility, ushort id)
            : base(extensibility, id)
        {
            this.DisableDuringExecution = true;
        }


        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            lock (this._syncLock)
            {
                this.enabled = !this.enabled;

                //TODO: implement turn on/off logging
                //TODO: inform somewhere
                this.DisplayName = $"AW: {(enabled ? "disable" : "enable")} tracking";
            }
        }
    }
}
