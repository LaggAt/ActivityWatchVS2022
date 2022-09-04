using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Definitions;

namespace At.Lagg.ActivityWatchVS2022
{
    [Command(ID, "AW: disable tracking", placement: CommandPlacement.ExtensionsMenu)]
    [CommandIcon("AddItem", IconSettings.IconOnly)]
    public class DisableTrackingCommand : Command
    {
        #region Fields

        public const string ID = "At.Lagg.ActivityWatchVS2022.DisableTrackingCommand";

        private readonly object _syncLock = new object();
        private bool enabled = true;

        #endregion Fields

        #region CTor

        public DisableTrackingCommand(VisualStudioExtensibility extensibility, string name)
            : base(extensibility, name)
        {
            this.DisableDuringExecution = true;
        }

        #endregion CTor

        #region Methods

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

        #endregion Methods
    }
}
