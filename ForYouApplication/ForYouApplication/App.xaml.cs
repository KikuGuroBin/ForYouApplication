using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ForYouApplication
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            
            MainPage = new NavigationPage(new MainPage());

            /* MainPage = new NavigationPage(new MasterDetailPage1()); */
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
            System.Diagnostics.Debug.WriteLine("deg : Application.OnStart()");
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            System.Diagnostics.Debug.WriteLine("deg : Application.OnSleep()");
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            System.Diagnostics.Debug.WriteLine("deg : Application.OnResume()");
        }
	}
}
