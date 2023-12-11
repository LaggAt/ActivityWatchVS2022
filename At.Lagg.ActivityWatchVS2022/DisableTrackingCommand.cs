using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;

namespace At.Lagg.ActivityWatchVS2022
{
    //[Command(NAME, ID, "AW: disable tracking", placement: KnownCommandPlacement.ExtensionsMenu)]
    //[CommandIcon("AddItem", IconSettings.IconOnly)]
    [VisualStudioContribution]
    public class DisableTrackingCommand : Command
    {
        //public const ushort ID = 1;
        //public const string NAME = "At.Lagg.ActivityWatchVS2022.DisableTrackingCommand";

        private readonly object _syncLock = new object();
        private bool enabled = true;

        public DisableTrackingCommand(VisualStudioExtensibility extensibility)
            : base(extensibility)
        {
            this.DisableDuringExecution = true;
        }

        #region Methods

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("ActivityWatch: disable tracking")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
            Placements = new[] { CommandPlacement.KnownPlacements.ExtensionsMenu },
        };

        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lock (this._syncLock)
            {
                this.enabled = !this.enabled;

                //TODO: implement turn on/off logging
                //TODO: inform somewhere
                this.DisplayName = $"ActivityWatch: {(enabled ? "disable" : "enable")} tracking";
            }

            string promptMessage = $"{(enabled ? "Enabled" : "Disabled")} tracking to ActivityWatch.";
            await this.Extensibility.Shell().ShowPromptAsync(promptMessage, PromptOptions.OK, cancellationToken);
        }

        #endregion Methods
    }
}