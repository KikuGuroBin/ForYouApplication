using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    public class MyContentEventArgs
    {
        /* アクションの種類 */
        public int Action;

        /* スワイプの判定 */
        public bool SwipeFlag;

        /* スワイプした距離 */
        public float SwipeDistance;

        public MyContentEventArgs(int action, bool flag, float swipe)
        {
            Action = action;
            SwipeFlag = flag;
            SwipeDistance = swipe;
        }
    }
}