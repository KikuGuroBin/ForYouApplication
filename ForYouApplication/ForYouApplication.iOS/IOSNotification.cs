using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms;

using ForYouApplication;
using ForYouApplication.iOS;

[assembly:Dependency(typeof(IOSNotification))]
namespace ForYouApplication.iOS
{
    public class IOSNotification : INotification
    {
        public void ShowMessage(string message)
        {

        }
    }
}