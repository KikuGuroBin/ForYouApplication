using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    public class ManipulationDeltaRoutedEventArgs
    {
        public Delta_ Delta { get; set; }
        public object OriginalSource { get; set; }

        public ManipulationDeltaRoutedEventArgs(object source, double deltaX, double deltaY)
        {
            OriginalSource = source;
            Delta = new Delta_()
            {
                Translation = new Delta_.Translation_()
                {
                    X = deltaX,
                    Y = deltaY
                }
            };
        }

        public class Delta_
        {
            public Translation_ Translation { get; set; }

            public class Translation_
            {
                public double X { get; set; }
                public double Y { get; set; }
            }
        }
    }
}