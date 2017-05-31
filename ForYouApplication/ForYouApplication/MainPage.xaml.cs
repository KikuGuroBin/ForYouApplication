using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ForYouApplication
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            InitializeComponent();
		}

        private void Test()
        {
            this.Label1.Text = "ざまりうす"; /*追加　コミットのテスト*/
        }
	}
}
