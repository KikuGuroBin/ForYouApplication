using System;
using System.Collections.Generic;
using System.Text;

namespace ForYouApplication
{
    /* リモートホストに送るタグ用 */
    public enum TagConstants
    {
        /* 入力タグ */
        INPUT,
        /* 確定タグ */
        ENTER,
        /* 削除タグ */
        DELETE,
        /* 変換タグ */
        CONVERSION,
        /* ショートカットタグ */
        SHORTCUT,
        /* 切断タグ */
        ENDCONNECTION
    }

    /* enum TagConstantsの拡張クラス */
    public static class TagConstantsExpansion
    {
        public static string GetConstants(this TagConstants value)
        {
            string[] values = {"<INP>", "<ENT>", "<DEL>", "<CON>", "<SHO>", "<END>"};
            return values[(int)value];
        }
    }

    public class A
    {
        public void B()
        {
            TagConstants.INPUT.GetConstants();
        }
    }
}
