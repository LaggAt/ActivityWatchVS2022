using Microsoft.VisualStudio.Extensibility.Editor.UI;
using Microsoft.VisualStudio.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Extensibility.Editor;
using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft;

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
            await this._eventService.AddEvent(args.AfterTextView.RpcContract);
        }

        public async Task TextViewClosedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEvent(textView.RpcContract);
        }

        public async Task TextViewCreatedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEvent(textView.RpcContract);
        }
    }
}
