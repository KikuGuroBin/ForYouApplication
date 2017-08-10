using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    /* リモートホストに送るタグ用 */
    public enum TagConstants
    {
        /* 確定タグ */
        ENTER,
        /* デリートタグ */
        DELETE,
        /* バックスペースタグ */
        BACK,
        /* 変換タグ */
        CONV,
        /* コピータグ */
        COPY,
        /* カットタグ */
        CUT,
        /* ペーストタグ */
        PASTE,
        /* トラックパッドタグ */
        MOUSE,
        /* 切断タグ */
        END
    }

    /* TagConstantsの拡張クラス */
    public static class TagConstantsEx
    {
        private static string[] values = { "<ENT>", "<DEL>", "<BAC>", "<CON>", "<COP>", "<CUT>", "<PAS>", "<MOU>", "<END>" };

        public static string GetConstants(this TagConstants value)
        {
            return values[(int)value];
        }

        public static bool Contains(string text)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (text.IndexOf(values[i]) > -1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
