using System;
using Xamarin.Forms;

namespace ForYouApplication
{
    public class TrackPad : BoxView
    {
        /* レンダラーのコールバック */
        public EventHandler<ManipulationDeltaRoutedEventArgs> OnManipulationDelta;
        
        /* ドラッグの動作をセット */
        public TrackPad()
        {
            OnManipulationDelta += (s, e) =>
            {
                if (e.Action == 2)
                {
                    Rectangle rc = Bounds;
                    rc.X += e.Translation.X;
                    rc.Y += e.Translation.Y;
                    Layout(rc);
                }
            };
        }
    }
}
