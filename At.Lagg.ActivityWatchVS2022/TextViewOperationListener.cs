﻿using At.Lagg.ActivityWatchVS2022.Services;
using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Editor;
//using Microsoft.VisualStudio.Extensibility.Editor.UI;

namespace At.Lagg.ActivityWatchVS2022
{
    //[ExtensionPart(typeof(ITextViewLifetimeListener))]
    //[ExtensionPart(typeof(ITextViewChangedListener))]
    //[AppliesTo(ContentType = "text")]
    //[AppliesTo(ContentType = "code")]
    [VisualStudioContribution]                
    public class TextViewOperationListener :
        ExtensionPart, // This is the extension part base class containing infrastructure necessary to use VS services.
        ITextViewOpenClosedListener, // Indicates this part listens for text view lifetime events.
        ITextViewChangedListener // Indicates this part listens to text view changes.
    {
        private readonly EventService _eventService;

        public TextViewOperationListener(ExtensionCore container, VisualStudioExtensibility extensibilityObject,
            EventService eventService
        ) : base(container, extensibilityObject)
        {
            this._eventService = Requires.NotNull(eventService, nameof(eventService));
        }

        public TextViewExtensionConfiguration TextViewExtensionConfiguration => new()
        {
            // Indicates this part should only light up in C# files.
            //AppliesTo = new[] { DocumentFilter.FromDocumentType("CSharp") },
        };

        public async Task TextViewChangedAsync(TextViewChangedArgs args, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(args.AfterTextView.RpcContract);
        }

        public async Task TextViewClosedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(textView.RpcContract);
        }

        public async Task TextViewOpenedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            await this._eventService.AddEventAsync(textView.RpcContract);
        }
    }
}
