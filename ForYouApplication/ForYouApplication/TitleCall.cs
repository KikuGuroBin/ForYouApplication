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
        Image titleImage;
        double a;

        public TitleCall()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            a =this.Width;
            var abs = new AbsoluteLayout();

            titleImage = new Image
            {
                Source = "titlessss.png",
                WidthRequest = a
            };

            iconImage = new Image
            {
                Source = "title.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            AbsoluteLayout.SetLayoutFlags(iconImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(iconImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(titleImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(titleImage,
                new Rectangle(0.5, 0.7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            abs.Children.Add(titleImage);
            abs.Children.Add(iconImage);

            this.BackgroundColor = Color.FromHex("#2296f4");
            this.Content = abs;
        }

        public LayoutOptions FillAndExpand { get; private set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await iconImage.ScaleTo(1, 1500); //初期化などの時間のかかる処理
            await iconImage.ScaleTo(0.8, 400, Easing.Linear);
            await iconImage.ScaleTo(1, 400, Easing.Linear);
            await iconImage.ScaleTo(0.8, 400, Easing.Linear);
            await iconImage.ScaleTo(150, 800, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new MainPage());

        }
    }
}



   