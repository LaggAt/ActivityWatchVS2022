using Microsoft.VisualStudio.Extensibility.Editor.UI;
using Microsoft.VisualStudio.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Extensibility.Editor;

namespace At.Lagg.ActivityWatchVS2022
{
    [ExtensionPart(typeof(ITextViewLifetimeListener))]
    [ExtensionPart(typeof(ITextViewChangedListener))]
    [AppliesTo(ContentType = "text")]
    [AppliesTo(ContentType = "code")]
    public sealed class TextViewOperationListener : ExtensionPart, ITextViewLifetimeListener, ITextViewChangedListener
    {
        public TextViewOperationListener(ExtensionCore container, VisualStudioExtensibility extensibilityObject) 
            : base(container, extensibilityObject)
        {
        }

        public async Task TextViewChangedAsync(TextViewChangedArgs args, CancellationToken cancellationToken)
        {
            string fileName = args.AfterTextView.RpcContract.Document.Uri.AbsoluteUri;

        }

        public async Task TextViewClosedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            string fileName = textView.RpcContract.Document.Uri.AbsoluteUri;

        }

        public async Task TextViewCreatedAsync(ITextView textView, CancellationToken cancellationToken)
        {
            string fileName = textView.RpcContract.Document.Uri.AbsoluteUri;
        }
    }
}
