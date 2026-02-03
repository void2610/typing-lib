using System.Collections.Generic;
using System.Linq;

namespace Void2610.TypingLib.Services
{
    /// <summary>
    /// ローマ字→ひらがな変換テーブル
    /// </summary>
    public static class RomajiTable
    {
        /// <summary>
        /// ローマ字からひらがなへの変換マッピング
        /// </summary>
        private static readonly Dictionary<string, string> RomajiToHiragana = new Dictionary<string, string>
        {
            // 母音
            { "a", "あ" }, { "i", "い" }, { "u", "う" }, { "e", "え" }, { "o", "お" },

            // か行
            { "ka", "か" }, { "ki", "き" }, { "ku", "く" }, { "ke", "け" }, { "ko", "こ" },
            { "ca", "か" }, { "cu", "く" }, { "co", "こ" },

            // さ行
            { "sa", "さ" }, { "si", "し" }, { "su", "す" }, { "se", "せ" }, { "so", "そ" },
            { "shi", "し" },

            // た行
            { "ta", "た" }, { "ti", "ち" }, { "tu", "つ" }, { "te", "て" }, { "to", "と" },
            { "chi", "ち" }, { "tsu", "つ" },

            // な行
            { "na", "な" }, { "ni", "に" }, { "nu", "ぬ" }, { "ne", "ね" }, { "no", "の" },

            // は行
            { "ha", "は" }, { "hi", "ひ" }, { "hu", "ふ" }, { "he", "へ" }, { "ho", "ほ" },
            { "fu", "ふ" },

            // ま行
            { "ma", "ま" }, { "mi", "み" }, { "mu", "む" }, { "me", "め" }, { "mo", "も" },

            // や行
            { "ya", "や" }, { "yu", "ゆ" }, { "yo", "よ" },

            // ら行
            { "ra", "ら" }, { "ri", "り" }, { "ru", "る" }, { "re", "れ" }, { "ro", "ろ" },

            // わ行
            { "wa", "わ" }, { "wi", "うぃ" }, { "we", "うぇ" }, { "wo", "を" },

            // ん
            { "nn", "ん" }, { "n'", "ん" }, { "xn", "ん" },

            // が行
            { "ga", "が" }, { "gi", "ぎ" }, { "gu", "ぐ" }, { "ge", "げ" }, { "go", "ご" },

            // ざ行
            { "za", "ざ" }, { "zi", "じ" }, { "zu", "ず" }, { "ze", "ぜ" }, { "zo", "ぞ" },
            { "ji", "じ" },

            // だ行
            { "da", "だ" }, { "di", "ぢ" }, { "du", "づ" }, { "de", "で" }, { "do", "ど" },

            // ば行
            { "ba", "ば" }, { "bi", "び" }, { "bu", "ぶ" }, { "be", "べ" }, { "bo", "ぼ" },

            // ぱ行
            { "pa", "ぱ" }, { "pi", "ぴ" }, { "pu", "ぷ" }, { "pe", "ぺ" }, { "po", "ぽ" },

            // 拗音（きゃ、きゅ、きょ など）
            { "kya", "きゃ" }, { "kyi", "きぃ" }, { "kyu", "きゅ" }, { "kye", "きぇ" }, { "kyo", "きょ" },
            { "gya", "ぎゃ" }, { "gyi", "ぎぃ" }, { "gyu", "ぎゅ" }, { "gye", "ぎぇ" }, { "gyo", "ぎょ" },
            { "sya", "しゃ" }, { "syi", "しぃ" }, { "syu", "しゅ" }, { "sye", "しぇ" }, { "syo", "しょ" },
            { "sha", "しゃ" }, { "shu", "しゅ" }, { "she", "しぇ" }, { "sho", "しょ" },
            { "zya", "じゃ" }, { "zyi", "じぃ" }, { "zyu", "じゅ" }, { "zye", "じぇ" }, { "zyo", "じょ" },
            { "ja", "じゃ" }, { "ju", "じゅ" }, { "je", "じぇ" }, { "jo", "じょ" },
            { "jya", "じゃ" }, { "jyi", "じぃ" }, { "jyu", "じゅ" }, { "jye", "じぇ" }, { "jyo", "じょ" },
            { "tya", "ちゃ" }, { "tyi", "ちぃ" }, { "tyu", "ちゅ" }, { "tye", "ちぇ" }, { "tyo", "ちょ" },
            { "cha", "ちゃ" }, { "chu", "ちゅ" }, { "che", "ちぇ" }, { "cho", "ちょ" },
            { "cya", "ちゃ" }, { "cyi", "ちぃ" }, { "cyu", "ちゅ" }, { "cye", "ちぇ" }, { "cyo", "ちょ" },
            { "dya", "ぢゃ" }, { "dyi", "ぢぃ" }, { "dyu", "ぢゅ" }, { "dye", "ぢぇ" }, { "dyo", "ぢょ" },
            { "nya", "にゃ" }, { "nyi", "にぃ" }, { "nyu", "にゅ" }, { "nye", "にぇ" }, { "nyo", "にょ" },
            { "hya", "ひゃ" }, { "hyi", "ひぃ" }, { "hyu", "ひゅ" }, { "hye", "ひぇ" }, { "hyo", "ひょ" },
            { "bya", "びゃ" }, { "byi", "びぃ" }, { "byu", "びゅ" }, { "bye", "びぇ" }, { "byo", "びょ" },
            { "pya", "ぴゃ" }, { "pyi", "ぴぃ" }, { "pyu", "ぴゅ" }, { "pye", "ぴぇ" }, { "pyo", "ぴょ" },
            { "mya", "みゃ" }, { "myi", "みぃ" }, { "myu", "みゅ" }, { "mye", "みぇ" }, { "myo", "みょ" },
            { "rya", "りゃ" }, { "ryi", "りぃ" }, { "ryu", "りゅ" }, { "rye", "りぇ" }, { "ryo", "りょ" },

            // ファ行など
            { "fa", "ふぁ" }, { "fi", "ふぃ" }, { "fe", "ふぇ" }, { "fo", "ふぉ" },
            { "fya", "ふゃ" }, { "fyu", "ふゅ" }, { "fyo", "ふょ" },

            // ティ、ディなど
            { "thi", "てぃ" }, { "thu", "てゅ" },
            { "dhi", "でぃ" }, { "dhu", "でゅ" },

            // トゥ、ドゥ
            { "twu", "とぅ" }, { "dwu", "どぅ" },

            // ウィ、ウェ、ウォ
            { "whi", "うぃ" }, { "whe", "うぇ" }, { "who", "うぉ" },

            // クァ行
            { "qa", "くぁ" }, { "qi", "くぃ" }, { "qe", "くぇ" }, { "qo", "くぉ" },
            { "qwa", "くぁ" }, { "qwi", "くぃ" }, { "qwu", "くぅ" }, { "qwe", "くぇ" }, { "qwo", "くぉ" },
            { "kwa", "くぁ" }, { "kwi", "くぃ" }, { "kwu", "くぅ" }, { "kwe", "くぇ" }, { "kwo", "くぉ" },

            // グァ行
            { "gwa", "ぐぁ" }, { "gwi", "ぐぃ" }, { "gwu", "ぐぅ" }, { "gwe", "ぐぇ" }, { "gwo", "ぐぉ" },

            // ツァ行
            { "tsa", "つぁ" }, { "tsi", "つぃ" }, { "tse", "つぇ" }, { "tso", "つぉ" },

            // 小文字（明示的入力）
            { "xa", "ぁ" }, { "xi", "ぃ" }, { "xu", "ぅ" }, { "xe", "ぇ" }, { "xo", "ぉ" },
            { "la", "ぁ" }, { "li", "ぃ" }, { "lu", "ぅ" }, { "le", "ぇ" }, { "lo", "ぉ" },
            { "xya", "ゃ" }, { "xyu", "ゅ" }, { "xyo", "ょ" },
            { "lya", "ゃ" }, { "lyu", "ゅ" }, { "lyo", "ょ" },
            { "xtu", "っ" }, { "xtsu", "っ" }, { "ltu", "っ" }, { "ltsu", "っ" },
            { "xwa", "ゎ" }, { "lwa", "ゎ" },
            { "xka", "ゕ" }, { "xke", "ゖ" }, { "lka", "ゕ" }, { "lke", "ゖ" },

            // ヴ行
            { "va", "ゔぁ" }, { "vi", "ゔぃ" }, { "vu", "ゔ" }, { "ve", "ゔぇ" }, { "vo", "ゔぉ" },
            { "vya", "ゔゃ" }, { "vyu", "ゔゅ" }, { "vyo", "ゔょ" },

            // その他
            { "-", "ー" },
        };

        /// <summary>
        /// 促音（っ）を生成する子音重ね
        /// これらの子音が連続した場合、最初の子音を「っ」に変換
        /// </summary>
        private static readonly HashSet<char> SokuonConsonants = new HashSet<char>
        {
            'k', 'g', 's', 'z', 't', 'd', 'h', 'f', 'b', 'p', 'm', 'r', 'w', 'j', 'c', 'v', 'y'
        };

        /// <summary>
        /// 有効なローマ字プレフィックスのキャッシュ
        /// </summary>
        private static readonly HashSet<string> ValidPrefixes;

        /// <summary>
        /// 静的コンストラクタでプレフィックスキャッシュを生成
        /// </summary>
        static RomajiTable()
        {
            ValidPrefixes = new HashSet<string>();
            foreach (var key in RomajiToHiragana.Keys)
            {
                // 各キーのすべてのプレフィックスを追加
                for (int i = 1; i <= key.Length; i++)
                {
                    ValidPrefixes.Add(key.Substring(0, i));
                }
            }

            // 「n」単独も有効なプレフィックス（「ん」または「な」行の開始）
            ValidPrefixes.Add("n");

            // 促音の子音重ねパターンを追加
            foreach (var c in SokuonConsonants)
            {
                ValidPrefixes.Add(c.ToString() + c);
            }
        }

        /// <summary>
        /// ローマ字をひらがなに変換する
        /// </summary>
        /// <param name="romaji">ローマ字</param>
        /// <param name="hiragana">変換されたひらがな</param>
        /// <returns>変換できた場合はtrue</returns>
        public static bool TryConvert(string romaji, out string hiragana)
        {
            if (string.IsNullOrEmpty(romaji))
            {
                hiragana = null;
                return false;
            }

            // 直接変換を試みる
            if (RomajiToHiragana.TryGetValue(romaji, out hiragana))
            {
                return true;
            }

            // 促音（子音重ね）の処理: "kk" → "っk", "tt" → "っt" など
            if (romaji.Length >= 2 && IsSokuonPattern(romaji, out var sokuonResult))
            {
                hiragana = sokuonResult;
                return true;
            }

            hiragana = null;
            return false;
        }

        /// <summary>
        /// 促音パターンかどうかを判定し、変換結果を返す
        /// </summary>
        /// <param name="romaji">ローマ字</param>
        /// <param name="result">変換結果（「っ」+残りのひらがな）</param>
        /// <returns>促音パターンで変換できた場合はtrue</returns>
        private static bool IsSokuonPattern(string romaji, out string result)
        {
            result = null;

            if (romaji.Length < 2)
            {
                return false;
            }

            var firstChar = romaji[0];
            var secondChar = romaji[1];

            // 同じ子音が連続していて、かつ促音を生成する子音の場合
            // 例: "kka" → "っか", "tta" → "った"
            if (firstChar == secondChar && SokuonConsonants.Contains(firstChar))
            {
                // 残りの部分（最初の子音を除いた部分）を変換
                var remaining = romaji.Substring(1);
                if (RomajiToHiragana.TryGetValue(remaining, out var convertedRemaining))
                {
                    result = "っ" + convertedRemaining;
                    return true;
                }
            }

            // "tch" → "っち" パターン（"cchi" の代わりに "tchi" を使う場合）
            if (romaji.Length >= 3 && romaji.StartsWith("tch"))
            {
                var remaining = "ch" + romaji.Substring(3);
                if (RomajiToHiragana.TryGetValue(remaining, out var convertedRemaining))
                {
                    result = "っ" + convertedRemaining;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 入力途中のローマ字として有効かどうかを判定する
        /// </summary>
        /// <param name="romaji">ローマ字</param>
        /// <returns>有効なプレフィックスの場合はtrue</returns>
        public static bool IsValidPrefix(string romaji)
        {
            if (string.IsNullOrEmpty(romaji))
            {
                return false;
            }

            // 直接プレフィックスとして登録されているか
            if (ValidPrefixes.Contains(romaji))
            {
                return true;
            }

            // 促音の途中パターン: 子音 + 有効なプレフィックス
            // 例: "kk" は "kka" の途中なので有効
            if (romaji.Length >= 2)
            {
                var firstChar = romaji[0];
                if (SokuonConsonants.Contains(firstChar) && romaji[1] == firstChar)
                {
                    // 残りの部分が有効なプレフィックスかどうか
                    var remaining = romaji.Substring(1);
                    if (ValidPrefixes.Contains(remaining))
                    {
                        return true;
                    }
                }

                // "tch" パターンの途中
                if (romaji.StartsWith("tc"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 「n」の後に続く文字が「ん」を確定させるかどうか
        /// </summary>
        /// <param name="nextChar">次の文字</param>
        /// <returns>「ん」を確定させる場合はtrue</returns>
        public static bool IsNFollowedByConsonant(char nextChar)
        {
            // 母音と'y'以外なら「ん」を確定
            // 'n'の場合は"nn"で「ん」になるので別処理
            return nextChar != 'a' && nextChar != 'i' && nextChar != 'u' &&
                   nextChar != 'e' && nextChar != 'o' && nextChar != 'y' && nextChar != 'n';
        }
    }
}
