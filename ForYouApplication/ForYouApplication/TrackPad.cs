using System;
using Xamarin.Forms;

namespace ForYouApplication
{
    public class TrackPad : BoxView
    {
        /* レンダラーのコールバック */
        public EventHandler<ManipulationDeltaRoutedEventArgs> OnManipulationDelta;
        
        /* ドラッグの動作をセット */
        public void SetEvent()
        {
            OnManipulationDelta += (s, e) =>
            {
                Rectangle rc = Bounds;
                rc.X += e.Translation.X;
                rc.Y += e.Translation.Y;
                Layout(rc);
            };
        }
    }
}
