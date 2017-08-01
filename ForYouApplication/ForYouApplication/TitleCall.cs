using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ForYouApplication
{
    public class TitleCall : ContentPage
    {
        Image iconImage;

        public TitleCall()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var abs = new AbsoluteLayout();

            iconImage = new Image
            {
                Source = "rrrrr.png",
                WidthRequest = 100,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutFlags(iconImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(iconImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            abs.Children.Add(iconImage);

            this.BackgroundColor = Color.FromHex("#2296f4");
            this.Content = abs;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await iconImage.ScaleTo(1, 1500); //初期化などの時間のかかる処理
            await iconImage.ScaleTo(0.8, 400, Easing.Linear);
            await iconImage.ScaleTo(1, 400, Easing.Linear);
            await iconImage.ScaleTo(0.8, 400, Easing.Linear);
            await iconImage.ScaleTo(100, 1100, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new MainPage());

        }
    }
}



   