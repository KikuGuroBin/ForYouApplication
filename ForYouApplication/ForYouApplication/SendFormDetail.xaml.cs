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

        /* 右からのショートカットメニュー用 */
        public StackLayout ShortCutPane;
        public StackLayout SubKeyPane;
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

        double LabelWidth;
        
        /*ラベルたち*/
        public Label Uplabel1;
        public Label Backlabel1;
        public Label Leftlabel1;
        public Label Downlabel1;
        public Label Rightlabel1;
        public Label Shift;
        public Label Tab;
        public Label ChangeTab;
        public Label Enter;

        public Color LightYellow = Color.FromRgb(0xFF, 0xEF, 0x85);

        public bool ShiftFlag;

        public SendFormDetail()
        {
            InitializeComponent();

            /* iOSだけ、上部に余白をとる */
            Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? 20 : 0, 0, 0);

            /* アイテムをSendFormPageで参照できるようにメンバ変数に格納 */
            Editor = SendText;
            ShortCutPane = SidePane;
            Uplabel1 = UpLabel;
            Backlabel1 = BackLabel;
            Leftlabel1 = LeftLabel;
            Downlabel1 = DownLabel;
            Rightlabel1 = RightLabel;
            Shift = Swift;
            Tab = Tub; 
            ChangeTab = ChanTab;
            Enter = Ender;

            /* シュートカットメニュー用パネルの設定 */
            CreatePanel();

            /* データバインディング */
            BindingContext = new ShortCutListViewModel();
            ShortCutList = ShortList;
        }
      
        /* 画面サイズが変更されたときのイベント */
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            /* 初回だけ処理を行う */
            if (FirstOrder)
            {
                /* メインのレイアウトの幅、高さを設定 */
                SendItems.WidthRequest = width;
                SendItems.HeightRequest = height - 70;

                /* ラベルなどに設定する幅、高さを求める */
                LabelWidth = width / 5;
                double labelHeight = LabelWidth / 3 * 2;

                /* ラベルの幅を設定 */
                Shift.WidthRequest = LabelWidth;
                UpLabel.WidthRequest = LabelWidth;
                Tab.WidthRequest = LabelWidth;
                BackLabel.WidthRequest = LabelWidth;
                ChangeTab.WidthRequest = LabelWidth;
                LeftLabel.WidthRequest = LabelWidth;
                DownLabel.WidthRequest = LabelWidth;
                RightLabel.WidthRequest = LabelWidth;
                ShotcutLabel.WidthRequest = LabelWidth;
                Enter.WidthRequest = LabelWidth;

                Scroll.WidthRequest = width;
                Scroll.HeightRequest = labelHeight;

                /* ラベルの高さを設定 */
                Shift.HeightRequest = labelHeight;
                UpLabel.HeightRequest = labelHeight;
                Tab.HeightRequest = labelHeight;
                BackLabel.HeightRequest = labelHeight;
                ChangeTab.HeightRequest = labelHeight;
                LeftLabel.HeightRequest = labelHeight;
                DownLabel.HeightRequest = labelHeight;
                RightLabel.HeightRequest = labelHeight;
                ShotcutLabel.HeightRequest = labelHeight;
                Enter.HeightRequest = labelHeight;

                MyBox.TranslationY = 300;
                
                EditLay.HeightRequest = labelHeight * 1.5;

                /* エディターにフォーカスを合わせる */
                SendText.Focus();

                /* 次回以降呼ばれないようにする */
                FirstOrder = false;
            }
        }

        /* ショートカットメニュー用パネル組み込み用 */
        private void CreatePanel()
        {
            /* 生成したパネルをメインのレイアウトに組み込む */
            MainLayout.Children.Add(ShortCutPane,
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
            /*
            MainLayout.Children.Add(KeyPane,
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
            */
        }

        private async void AnimatePanel()
        {
            // swap the state
            this.PanelShowing = !PanelShowing;

            // show or hide the panel
            if (this.PanelShowing)
            {
                // layout the panel to slide out
                var rect = new Rectangle(MainLayout.Width, ShortCutPane.Y, ShortCutPane.Width, ShortCutPane.Height / 2);

                /*
                var rect2 = new Rectangle(-ShortCutPane.Width, MainLayout.Y, MainLayout.Width + ShortCutPane.Width, MainLayout.Height);
                */

                await ShortCutPane.LayoutTo(rect, 100, Easing.CubicIn);

                EditLay.WidthRequest = EditLay.Width - ShortCutPane.Width - 20;
            }
            else
            {
                // layout the panel to slide in
                var rect = new Rectangle(MainLayout.Width, ShortCutPane.Y, ShortCutPane.Width, ShortCutPane.Height * 2);
                var rect2 = new Rectangle(0, 0, MainLayout.Width - ShortCutPane.Width, MainLayout.Height);

                await ShortCutPane.LayoutTo(rect, 50, Easing.CubicOut);
                //await MainLayout.LayoutTo(rect2, 50, Easing.CubicOut);
            }
        }

        /* ショートカットラベルタップ用のイベント */
        private void ShortCutTap(object sender, EventArgs args)
        {
            AnimatePanel();
        }

        /* シフトラベルタップ時のイベント */
        private void ShiftTap(object sender, EventArgs args)
        {
            /* シフトモードOFF時には使用できるラベルの背景色を変える */
            if (!ShiftFlag)
            {
                UpLabel.BackgroundColor = LightYellow;
                DownLabel.BackgroundColor = LightYellow;
                RightLabel.BackgroundColor = LightYellow;
                LeftLabel.BackgroundColor = LightYellow;
                Tab.BackgroundColor = LightYellow;

                ShiftFlag = true;
            }
            else
            {
                UpLabel.BackgroundColor = Color.White;
                DownLabel.BackgroundColor = Color.White;
                RightLabel.BackgroundColor = Color.White;
                LeftLabel.BackgroundColor = Color.White;
                Tab.BackgroundColor = Color.White;

                ShiftFlag = false;
            }
        }

        class ShortCutListViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ShortCutListItem> ShortCutItems { get; set; }

            public ShortCutListViewModel()
            {
                ShortCutItems = new ObservableCollection<ShortCutListItem>(new[]
                {
                    new ShortCutListItem { Id = 10, Title = "コピー", Send = "<COP>" },
                    new ShortCutListItem { Id = 11, Title = "カット", Send = "<CUT>" },
                    new ShortCutListItem { Id = 12, Title = "ペースト", Send = "<PAS>" },
                    new ShortCutListItem { Id = 20, Title = "全選択", Send = "<ALL>" },
                    new ShortCutListItem { Id = 13, Title = "過去", Send = "<BEF>" },
                    new ShortCutListItem { Id = 14, Title = "未来", Send = "<AFT>" },
                    new ShortCutListItem { Id = 15, Title = "検索", Send = "<SEA>" },
                    new ShortCutListItem { Id = 16, Title = "開く", Send = "<OPN>" },
                    new ShortCutListItem { Id = -1, Title = "ブランク", Send = "<>" },
                    new ShortCutListItem { Id = 18, Title = "名前を付けて保存", Send = "<NSA>" },
                    new ShortCutListItem { Id = 19, Title = "上書き保存", Send = "<SAV>" },
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