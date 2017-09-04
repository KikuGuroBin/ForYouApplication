using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;

using ForYouApplication.Droid;

[assembly:Dependency(typeof(DroidNotification))]
namespace ForYouApplication.Droid
{
    public class DroidNotification : INotification
    {
        public void ShowMessage(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}