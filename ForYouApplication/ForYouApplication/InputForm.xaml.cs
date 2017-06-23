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
	public partial class InputForm : ContentPage
	{
        AsyncTcpClient client;

		public InputForm (AsyncTcpClient client)
		{
			InitializeComponent ();

            this.client = client;
		}

        private void OnClick(Object sender, EventArgs args)
        {
            string data = SendText.Text;

            client.Send("<TEST>", data);
        }
	}
}