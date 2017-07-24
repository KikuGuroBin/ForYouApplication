using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendFormDetail : ContentPage
    {
        public Editor Editor;
        public Button BackButton;

        /* OnSizeAllocated制御用 */
        private bool FirstOrder = true;

        /* ---右からのショートカットメニュー用--- */
        private StackLayout Panel;
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
            
            BackButton = Back;
            
            ShortCut.Clicked += (s, e) =>
            {
                AnimatePanel();
            };
            
            ShortCutList = new ListView();
            InitializeTemplate();

            /* ショートカットメニュー用パネル組み込み */
            CreatePanel();
        }

        private void InitializeTemplate()
        {
            DataTemplate data = new DataTemplate(() =>
            {
                StackLayout layout = new StackLayout();

                Label label = new Label();
                label.SetBinding(
                    Label.TextProperty,
                    "Title",
                    BindingMode.TwoWay,
                    null,
                    null
                );

                layout.Children.Add(label);

                return new ViewCell() { View = layout };
            });

            ShortCutList.ItemTemplate = data;
        }

        /* 画面サイズが変わったときに呼ばれる */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (FirstOrder)
            {
                SendItems.WidthRequest = width;
                SendItems.HeightRequest = height - 70;

                FirstOrder = false;
            }
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
                        },
                        */
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
                /* hide all children
                foreach (var child in Panel.Children)
                {
                    child.Scale = 0;
                }
                */

                // layout the panel to slide out
                //var rect = new Rectangle(MainLayout.Width, Panel.Y, Panel.Width, Panel.Height);
                var rect2 = new Rectangle(-Panel.Width, MainLayout.Y, MainLayout.Width, MainLayout.Height);

                //await Panel.LayoutTo(rect, 100, Easing.CubicIn);
                await MainLayout.LayoutTo(rect2, 100, Easing.CubicIn);
                
                /* scale in the children for the panel
                foreach (var child in Panel.Children)
                {
                    await child.ScaleTo(1.2, 50, Easing.CubicIn);
                    await child.ScaleTo(1, 50, Easing.CubicOut);
                }
                */
            }
            else
            {
                // layout the panel to slide in
                //var rect = new Rectangle(MainLayout.Width, Panel.Y, Panel.Width, Panel.Height);
                var rect2 = new Rectangle(0, 0, MainLayout.Width, MainLayout.Height);

                //await Panel.LayoutTo(rect, 100, Easing.CubicOut);
                await MainLayout.LayoutTo(rect2, 100, Easing.CubicOut);

                /* hide all children
                foreach (var child in Panel.Children)
                {
                    child.Scale = 0;
                }
                */
            }
        }

        class ShortCutListViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ShortCutListItem> MenuItems { get; set; }

            public ShortCutListViewModel()
            {
                MenuItems = new ObservableCollection<ShortCutListItem>(new[]
                {
                    new ShortCutListItem { Id = 0, Title = "コピー" },
                    new ShortCutListItem { Id = 1, Title = "カット" },
                    new ShortCutListItem { Id = 2, Title = "ペースト" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}