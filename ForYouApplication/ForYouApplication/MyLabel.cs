using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace ForYouApplication
{
    public class MyLabel : Label
    {
        public EventHandler<MyLabelEventArgs> Touch;

        public MyLabel()
        {
            Touch = (s, e) => {};
        }
    }
}
