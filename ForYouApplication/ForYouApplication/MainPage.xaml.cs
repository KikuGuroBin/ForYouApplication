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

        /*2017/5/31 追加
           ボタン押下に呼び出される*/
        private void OnClicked(Object sender, EventArgs args)
        {

        }

        private void Test()
        {
            this.Label1.Text = "ざまりうす"; /*追加　コミットのテスト*/
        }
	}
}
