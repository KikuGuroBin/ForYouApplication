using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForYouApplication
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InputForm : ContentPage
	{
        private AsyncTcpClient Client;

        private Socket socket;

		public InputForm (AsyncTcpClient client)
		{
			InitializeComponent ();

            this.Client = client;
		}

        public InputForm(Socket socket)
        {
            InitializeComponent();

            this.socket = socket;
        }

        private void OnClick(Object sender, EventArgs args)
        {
            if (sender.Equals(SendButton))
            {
                string data = SendText.Text;

                Client.Send("<TEST>", data);
            }
            else if (sender.Equals(DisconnButton))
            {
                Client.Disconnect();
            }
        }

        private void OnClick2(Object sender, EventArgs args)
        {
            if (sender.Equals(Send))
            {

            }
            else if (sender.Equals(Disconn))
            {
                socket.Close();
            }
        }
	}
}