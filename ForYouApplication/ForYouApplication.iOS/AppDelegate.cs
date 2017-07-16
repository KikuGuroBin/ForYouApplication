﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace ForYouApplication.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

            /* QRコード読み取り用 */
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            LoadApplication (new ForYouApplication.App ());

			return base.FinishedLaunching (app, options);
		}

        public override void WillTerminate(UIApplication uiApplication)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("WillTerminate. AppState={0}", uiApplication.ApplicationState.ToString()));

            base.WillTerminate(uiApplication);
        }
    }
}
