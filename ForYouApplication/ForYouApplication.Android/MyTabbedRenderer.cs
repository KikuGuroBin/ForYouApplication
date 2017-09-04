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
using Xamarin.Forms.Platform.Android;

using ForYouApplication;
using ForYouApplication.Droid;

[assembly:ExportRenderer(typeof(MyTabbedPage), typeof(MyTabbedRenderer))]
namespace ForYouApplication.Droid
{
    public class MyTabbedRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            
        }
    }
}