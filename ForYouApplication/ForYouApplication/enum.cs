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
        /* カーソルダウンタグ */
        DOWN,
        /* カーソルアップタグ */
        UP,
        /* カーソル移動タグ */
        MOVE,
        /* 切断タグ */
        END
    }

    /* TagConstantsの拡張クラス */
    public static class TagConstantsEx
    {
        private static string[] values = { "<ENT>", "<DEL>", "<BAC>", "<CON>", "<COP>", "<CUT>", "<PAS>", "<DOW>", "<UPP>", "<MOV>", "<END>" };

        public static string GetConstants(this TagConstants value)
        {
            return values[(int)value];
        }
    }
}
