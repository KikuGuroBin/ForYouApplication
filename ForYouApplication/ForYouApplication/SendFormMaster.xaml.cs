using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendFormMaster : ContentPage
    {
        public ListView ListView;
        public Label Header;

        public SendFormMaster()
        {
            InitializeComponent();

            BindingContext = new SendFormMasterViewModel();
            ListView  = MenuItemsListView;
            Header    = ListHeader;
        }

        class SendFormMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SendFormMenuItem> MenuItems { get; set; }
            
            public SendFormMasterViewModel()
            {
                MenuItems = new ObservableCollection<SendFormMenuItem>(new[]
                {
                    new SendFormMenuItem { Id = 0, Title = "切断" },
                    new SendFormMenuItem { Id = 1, Title = "トラックパッド" },
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