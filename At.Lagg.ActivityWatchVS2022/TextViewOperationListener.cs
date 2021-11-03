using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.Extensibility.Editor.UI;

namespace At.Lagg.ActivityWatchVS2022
{
    [ExtensionPart(typeof(ITextViewLifetimeListener))]
    [ExtensionPart(typeof(ITextViewChangedListener))]
    [AppliesTo(ContentType = "text")]
    [AppliesTo(ContentType = "code")]
    public class TextViewOperationListener : ExtensionPart, ITextViewLifetimeListener, ITextViewChangedListener
    {
        private readonly EventService _eventService;

        public TextViewOperationListener(ExtensionCore container, VisualStudioExtensibility extensibilityObject,
            EventService eventService
        ) : base(container, extensibilityObject)
        {
            this._eventService = Requires.NotNull(eventService, nameof(eventService));
        }

        public async Task TextViewChangedAsync(TextViewChangedArgs args, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(args.AfterTextView.RpcContract);
        }

        public async Task TextViewClosedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(textView.RpcContract);
        }

        public async Task TextViewCreatedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(textView.RpcContract);
        }
    }
}
