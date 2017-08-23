using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPageMaster : ContentPage
    {
        public ListView ListView;

        public StartPageMaster()
        {
            InitializeComponent();

            BindingContext = new StartPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class StartPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<StartPageMenuItem> MenuItems { get; set; }
            
            public StartPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<StartPageMenuItem>(new[]
                {
                    new StartPageMenuItem { Id = 0, Title = "Page 1" },
                    new StartPageMenuItem { Id = 1, Title = "Page 2" },
                    new StartPageMenuItem { Id = 2, Title = "Page 3" },
                    new StartPageMenuItem { Id = 3, Title = "Page 4" },
                    new StartPageMenuItem { Id = 4, Title = "Page 5" },
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