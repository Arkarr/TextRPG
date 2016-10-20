using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Text
{
    internal static class TextFormatter
    {
        public const char VARIABLE_IDENTIFIER = '$';
        public const int DEFAULT_DECIMALS_NUMBER = 2;

        public static readonly string VAR_CURRENT = VARIABLE_IDENTIFIER + "cur";
        public static readonly string VAR_MAX = VARIABLE_IDENTIFIER + "max";
        public static readonly string VAR_MIN = VARIABLE_IDENTIFIER + "min";
        public static readonly string VAR_PER = VARIABLE_IDENTIFIER + "per";

        public static string GetFormattedText(GameAttribute attr, string format)
        {
            //$cur.2 / $max% --> 54.75 / 120%
            // . before the decimals (or to notify the end of the declaration) (can be omitted if no decimals)
            int start = format.IndexOf(VARIABLE_IDENTIFIER);

            while (start != -1)
            {
                string var = format.Substring(start, 4);

                int index = 3;
                /*bool isDecimal = start + ++index < format.Length && format[start + index] == '.';
                int decimals = isDecimal && char.IsDigit(format[start + ++index]) ? int.Parse(format[start + index++].ToString()) : 0;

                string decimalsStr = "0.";
                for (int i = 0; i < decimals; i++)
                    decimalsStr += "0";*/

                int decimals = DEFAULT_DECIMALS_NUMBER;

                if (start + ++index < format.Length && format[start + index] == '.') // In case the number of decimals is precised
                {
                    decimals = char.IsDigit(format[start + ++index]) ? int.Parse(format[start + index++].ToString()) : 0;
                }

                string decimalsStr = "0.";
                for (int i = 0; i < decimals; i++)
                    decimalsStr += "0";

                string text = "";

                if (attr is DerivedGameAttribute)
                {
                    double value = 0.0;

                    if (var == VAR_CURRENT) value = (attr as DerivedGameAttribute).DoubelValue;
                    else if (var == VAR_MAX) value = attr.Max;
                    else if (var == VAR_MIN) value = attr.Min;

                    text = value.ToString(decimalsStr, CultureInfo.InvariantCulture);
                }
                else
                {
                    if (var == VAR_CURRENT) text = attr.Current.ToString(CultureInfo.InvariantCulture);
                    else if (var == VAR_MAX) text = attr.Max.ToString(CultureInfo.InvariantCulture);
                    else if (var == VAR_MIN) text = attr.Min.ToString(CultureInfo.InvariantCulture);
                    else if (attr is Resource && var == VAR_PER) text = (attr as Resource).Percentage.ToString(decimalsStr, CultureInfo.InvariantCulture);
                }

                format = format.Replace(format.Substring(start, index), text);
                start = format.IndexOf(VARIABLE_IDENTIFIER);
            }

            return format;
        }
    }
}
