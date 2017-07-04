using System;
using Xamarin.Forms;

namespace ForYouApplication
{
    public class Pad : Image
    {
        public event EventHandler LongPress;

        public void OnLongPress()
        {
            LongPress?.Invoke(this, new EventArgs());
        }
    }
}
