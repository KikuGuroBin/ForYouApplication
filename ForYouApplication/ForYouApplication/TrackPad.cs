using System;
using Xamarin.Forms;

namespace ForYouApplication
{
    public class TrackPad : BoxView
    {
        /* レンダラーのコールバック */
        public virtual void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            Rectangle rc = Bounds;
            rc.X += args.Delta.Translation.X;
            rc.Y += args.Delta.Translation.Y;
            Layout(rc);
        }
    }
}
