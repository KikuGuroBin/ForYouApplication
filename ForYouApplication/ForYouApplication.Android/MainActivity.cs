using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ZXing.Net.Mobile.Android;

using Xamarin.Forms.Platform.Android;

namespace ForYouApplication.Droid
{
	[Activity (Label = "ForYouApplication", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{

        protected override void OnCreate (Bundle bundle)
		{
            /* Xamarin.Forms用 */
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 
            
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new App ());
            
            System.Diagnostics.Debug.WriteLine("takumi-----------------Xamarin.Android-----------------");
		}

        public void OnClick(View v)
        {
            System.Diagnostics.Debug.WriteLine("Click");
        }

        /* Android6.0以上の端末用のランタイムパーミッションの設定 */
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}