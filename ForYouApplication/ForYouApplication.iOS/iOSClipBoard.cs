using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using ForYouApplication.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(IOSClipBoard))]

namespace ForYouApplication.iOS
{
    public class IOSClipBoard : IClipBoard
    {
        public bool SaveClipBoard(string text)
        {
            UIPasteboard.General.SetValue(new NSString(text), MobileCoreServices.UTType.Text);
            return true;
        }
    }
}