using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientTabbedPage : TabbedPage
    {
        public ClientTabbedPage()
        {
            InitializeComponent();
        }

        public ClientTabbedPage(TcpClient client)
        {
            InitializeComponent();

            AsyncTcpClient c = new AsyncTcpClient(client);

            Mouse.SetClient(c);
            Flick.SetClient(c);
        }

        /* バックボタン押下時のイベント */
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}