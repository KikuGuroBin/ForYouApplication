using System;
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

        /* OnSizeAllocated制御用 */
        private bool FirstOrder = true;

        /* ---右からのショートカットメニュー用--- */
        public StackLayout Panel;
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
        
        

        public ListView MenuItemsListView { get; private set; }

        /*ラベルたち*/
        public Label Uplabel1;
        public Label Backlabel1;
        public Label Leftlabel1;
        public Label Downlabel1;
        public Label Rightlabel1;

        /* ------------------------ */

        public SendFormDetail()
        {
            InitializeComponent();

            /* iOSだけ、上部に余白をとる */
            Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? 20 : 0, 0, 0);

            Editor = SendText;
       

            Panel = SidePane;
            CreatePanel();
            BindingContext = new ShortCutListViewModel();
            ShortCutList = ll9;

            Uplabel1 = UpLabel;
            Backlabel1 = BackLabel;
            Leftlabel1 = LeftLabel;
            Downlabel1 = DownLabel;
            Rightlabel1 = RightLabel;

            
            
        }
      
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (FirstOrder)
            {
                SendItems.WidthRequest = width;
                SendItems.HeightRequest = height - 70;

                FirstOrder = false;
                double a = width / 5;

                Brank_1.WidthRequest = a;
                UpLabel.WidthRequest = a;
                Brank_2.WidthRequest = a;
                BackLabel.WidthRequest = a;
                Brank_3.WidthRequest = a;

                LeftLabel.WidthRequest = a;
                DownLabel.WidthRequest = a;
                RightLabel.WidthRequest = a;
                ShotcutLabel.WidthRequest = a;
                Brank_4.WidthRequest = a;

                Brank_1.HeightRequest = a;
                UpLabel.HeightRequest = a;
                Brank_2.HeightRequest = a;
                BackLabel.HeightRequest = a;
                Brank_3.HeightRequest = a;

                LeftLabel.HeightRequest = a;
                DownLabel.HeightRequest = a;
                RightLabel.HeightRequest = a;
                ShotcutLabel.HeightRequest = a;
                Brank_4.HeightRequest = a;

                SendText.HeightRequest = a * 2;
                SendText.WidthRequest = a * 2;

                SendText.Focus();


            }
        }

        /* ショートカットメニュー用パネル組み込み用 */
        private void CreatePanel()
        {
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
                        */
                        ShortCutList,
                    },
                    Padding = 15,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Color.WhiteSmoke
                };

                
            }
        }

        private async void AnimatePanel()
        {
            // swap the state
            this.PanelShowing = !PanelShowing;

            // show or hide the panel
            if (this.PanelShowing)
            {
                /* hide all children*/
                foreach (var child in Panel.Children)
                {
                    child.Scale = 0;
                }
                

                // layout the panel to slide out
                var rect = new Rectangle(MainLayout.Width - Panel.Width, Panel.Y, Panel.Width, Panel.Height/2);
                var rect2 = new Rectangle(-Panel.Width, MainLayout.Y, MainLayout.Width, MainLayout.Height);

                await Panel.LayoutTo(rect, 100, Easing.CubicIn);
               // await MainLayout.LayoutTo(rect2, 100, Easing.CubicIn);
                
                /* scale in the children for the panel*/
                foreach (var child in Panel.Children)
                {
                    await child.ScaleTo(1.2, 50, Easing.CubicIn);
                    await child.ScaleTo(1, 50, Easing.CubicOut);
                }
                
            }
            else
            {
                // layout the panel to slide in
                var rect = new Rectangle(MainLayout.Width, Panel.Y, Panel.Width, Panel.Height*2);
                var rect2 = new Rectangle(0, 0, MainLayout.Width, MainLayout.Height);

                await Panel.LayoutTo(rect, 100, Easing.CubicOut);
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
                    
                    new ShortCutListItem { Id = 10, Title = "コピー" },
                    new ShortCutListItem { Id = 11, Title = "カット" },
                    new ShortCutListItem { Id = 12, Title = "ペースト" },
                    new ShortCutListItem { Id = 20, Title = "全選択" },
                    new ShortCutListItem { Id = 13, Title = "過去" },
                    new ShortCutListItem { Id = 14, Title = "未来" },
                    new ShortCutListItem { Id = 15, Title = "検索" },
                    new ShortCutListItem { Id = 16, Title = "開く" },
                    new ShortCutListItem { Id = 17, Title = "新規作成" },
                    new ShortCutListItem { Id = 18, Title = "名前を付けて保存" },
                    new ShortCutListItem { Id = 19, Title = "上書き保存" },
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

        private void Shbutton(object sender, EventArgs e)
        {
            AnimatePanel();
        }

    }
}