using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    public class MyBoxViewEventArgs
    {
        public int Action;

        public bool TouchFlag;

        public MyBoxViewEventArgs(int action, bool flag)
        {
            Action = action;
            TouchFlag = flag;
        }
    }
}
