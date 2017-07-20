using System;
using System.Text;

namespace ForYouApplication
{
    public class TextProcess
    {
        public static (int index, string send) SendContentDeicsion(string before, string after)
        {
            /* 各文字列を一文字ずつに分割して配列に格納 */
            char[] befArr = before.ToCharArray();
            char[] aftArr = after.ToCharArray();

            /* 各配列の要素数(文字数)格納 */
            int befLen = befArr.Length;
            int aftLen = aftArr.Length;

            /* 二つの配列のうち要素の少ないほうの値を下記のループ文の継続条件に使う */
            int lenMin = Math.Min(befLen, aftLen);

            /* ループ用インデックス */
            int preIndex = 0;
            /* 各配列の先頭から見ていき、最初に一致しなくなる場所を探す */
            for (; preIndex < lenMin; preIndex++)
            {
                char befChar = befArr[preIndex];
                char aftChar = aftArr[preIndex];

                if (!befChar.Equals(aftChar))
                {
                    break;
                }
            }

            /* 戻り値格納用 */
            string result = "";

            /* 一致しない文字がなかった場合 */
            if (preIndex == lenMin)
            {
                /* 変更後の文字数のほうが少なかった場合、バックスペースをされたと判定する */
                if (lenMin == aftLen)
                {
                    /* 加工 */
                    result = TextJoin(null, TagConstants.BACK.GetConstants(), "1");
                    return (-1, result);
                }
                /* 変更後の文字数のほうが多かった場合、入力がされたと判定する */
                else if (lenMin == befLen)
                {
                    /* 加工 */
                    result = TextJoin(after.Substring(lenMin), null);
                    return (-1, result);
                }
            }

            /* 要素数の多い方の文字の文字数を格納 */
            int max = Math.Max(befLen, aftLen);

            /* ループ用インデックス */
            int behiIndex = max - 1;

            /* 文字数の差を求める */
            (int be, int af) diff = befLen == aftLen ?
                                    (0, 0) : befLen < aftLen ?
                                            (aftLen - befLen, 0) : (0, befLen - aftLen);

            /* 配列を最後尾から見ていき、最初に一致しなかった場所を探す */
            for (; preIndex <= behiIndex; behiIndex--)
            {
                char befChar = befArr[behiIndex - diff.be];
                char aftChar = aftArr[behiIndex - diff.af];

                if (!befChar.Equals(aftChar))
                {
                    break;
                }
            }

            /* 文字列の一部分が削除されたと判定 */
            if (preIndex > behiIndex - Math.Min(diff.be, diff.af))
            {
                /* 加工 */
                result = TextJoin(null, TagConstants.DELETE.GetConstants(),
                                    Math.Max(diff.be, diff.af).ToString(),
                                        preIndex.ToString());
                return (-1, result);
            }

            /* ここまで来たら変換されたと判定する */

            /* 変換後の文字列の本当の要素番号 */
            int index = behiIndex - diff.af;

            /* 変換した箇所を取り出す */
            string conv = after.Substring(preIndex, behiIndex - preIndex - diff.af + 1);

            /* 加工 */
            result = TextJoin(null, TagConstants.CONV.GetConstants(),
                                behiIndex == max - 1 ? "0" : (befLen - (behiIndex - diff.be + 1)).ToString(),
                                    (behiIndex - preIndex - diff.be + 1).ToString(), conv);
            
            return (index, result);
        }

        /* 引数をリモートホストが処理できる形式に結合する */
        public static string TextJoin(string text, params string[] tag)
        {
            StringBuilder sb = new StringBuilder(tag ?[0] ?? "");
         
            /* タグとタグに付属する数値の結合 */
            for (int i = 1; tag != null && i < tag.Length; i++)
            {
                /* 文字列<>はタグと付属する数値の分割用 */
                sb.Append("<>");
                sb.Append(tag[i]);
            }
            
            sb.Append(text ?? "");

            return sb.ToString();
        }
    }
}