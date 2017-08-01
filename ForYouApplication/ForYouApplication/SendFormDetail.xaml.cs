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
                    
                    new ShortCutListItem { Id = 0, Title = "コピー" },
                    new ShortCutListItem { Id = 1, Title = "カット" },
                    new ShortCutListItem { Id = 2, Title = "ペースト" },
                    new ShortCutListItem { Id = 4, Title = "全範囲選択" },
                    new ShortCutListItem { Id = 5, Title = "元に戻す" },
                    new ShortCutListItem { Id = 6, Title = "" },
                    new ShortCutListItem { Id = 7, Title = "上書き保存" },
                    new ShortCutListItem { Id = 8, Title = "ペースト" },
                    new ShortCutListItem { Id = 9, Title = "名前を付けて保存" },
                    new ShortCutListItem { Id = 10, Title = "検索" },
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