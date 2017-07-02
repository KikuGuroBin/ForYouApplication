using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1Detail : ContentPage
    {

        public Editor Editor;
        public Button Disconnect;

        public MasterDetailPage1Detail()
        {
            InitializeComponent();

            /* iOSだけ、上部に余白をとる */
            Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? 20 : 0, 0, 0);

            Editor = SendText;
            Disconnect = Disconn;
        }
    }
}