using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    public class MyLabelEventArgs
    {
        /* モーションアクション */
        public int Action;

        public MyLabelEventArgs(int action)
        {
            Action = action;
        }
    }
}