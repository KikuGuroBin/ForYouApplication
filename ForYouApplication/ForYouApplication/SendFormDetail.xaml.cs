﻿using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendFormDetail : ContentPage
    {
        public Editor Editor;
        public Button button;

        /* ---右からのショートカットメニュー用--- */
        private StackLayout Panel;
        public Button CopyButton;
        public Button CutButton;
        public Button PasteButton;
        public ListView ShortCutList;
        private double PanelWidth = -1;
        private bool _PanelShowing = false;
        private bool PanelShowing
        {
            get
            {
                return _PanelShowing;
            }
            set
            {
                _PanelShowing = value;
            }
        }
        /* ------------------------ */

        public SendFormDetail()
        {
            InitializeComponent();

            /* iOSだけ、上部に余白をとる */
            Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? 20 : 0, 0, 0);

            Editor = SendText;

            button = Back;

            ShortCut.Clicked += (s, e) =>
            {
                AnimatePanel();
            };

            List<string> list = new List<string>()
            {
                "コピー",
                "カット",
                "ペースト",
            };

            ShortCutList = new ListView();
            ShortCutList.ItemsSource = list;
            
            /* ショートカットメニュー用パネル組み込み */
            CreatePanel();
        }

        /* ショートカットメニュー用パネル組み込み用 */
        private void CreatePanel()
        {
            if (Panel == null)
            {
                Panel = new StackLayout
                {
                    Children = {
                        /*
                        new Label {
                            Text = "Options",
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        },
                        new Label {
                            Text = "Options",
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        },
                        new Label {
                            Text = "Options",
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        },*/
                        ShortCutList,
                    },
                    Padding = 15,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Color.Blue
                };

                /* 生成したパネルをメインのレイアウトに組み込む */
                MainLayout.Children.Add(Panel,
                    Constraint.RelativeToParent((p) => {
                        return MainLayout.Width - (this.PanelShowing ? PanelWidth : 0);
                    }),
                    Constraint.RelativeToParent((p) => {
                        return 0;
                    }),
                    Constraint.RelativeToParent((p) => {
                        if (PanelWidth == -1)
                            PanelWidth = p.Width / 3;
                        return PanelWidth;
                    }),
                    Constraint.RelativeToParent((p) => {
                        return p.Height;
                    })
                );
            }
        }

        private async void AnimatePanel()
        {
            // swap the state
            this.PanelShowing = !PanelShowing;

            // show or hide the panel
            if (this.PanelShowing)
            {
                // hide all children
                foreach (var child in Panel.Children)
                {
                    child.Scale = 0;
                }

                // layout the panel to slide out
                var rect = new Rectangle(MainLayout.Width - Panel.Width, Panel.Y, Panel.Width, Panel.Height);
                
                await this.Panel.LayoutTo(rect, 250, Easing.CubicIn);
                
                // scale in the children for the panel
                foreach (var child in Panel.Children)
                {
                    await child.ScaleTo(1.2, 50, Easing.CubicIn);
                    await child.ScaleTo(1, 50, Easing.CubicOut);
                }
            }
            else
            {
                // layout the panel to slide in
                var rect = new Rectangle(MainLayout.Width, Panel.Y, Panel.Width, Panel.Height);
                await this.Panel.LayoutTo(rect, 200, Easing.CubicOut);

                // hide all children
                foreach (var child in Panel.Children)
                {
                    child.Scale = 0;
                }
            }
        }
    }
}