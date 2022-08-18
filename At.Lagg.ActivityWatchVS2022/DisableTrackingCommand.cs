using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Definitions;

namespace At.Lagg.ActivityWatchVS2022
{
    [Command(ID, "AW: disable tracking", placement: CommandPlacement.ExtensionsMenu)]
    [CommandIcon("AddItem", IconSettings.IconOnly)]
    public class DisableTrackingCommand : Command
    {
        public const string ID = "At.Lagg.ActivityWatchVS2022.DisableTrackingCommand";

        private readonly object _syncLock = new object();
        private bool enabled = true;

        public DisableTrackingCommand(VisualStudioExtensibility extensibility, string name)
            : base(extensibility, name)
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
