using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    /* 送信文字のサニタイジングを行う */
    public class Sanitizing
    {
        public static string TagSanitaize(string text)
        {
            for (int i = text.IndexOf("a"); i < 10; i++)
            {

            }

            return "aaa";
        }
    }
}