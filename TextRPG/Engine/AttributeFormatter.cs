using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    internal static class AttributeFormatter
    {
        public const char VARIABLE_IDENTIFIER = '$';

        public static readonly string VAR_CURRENT = VARIABLE_IDENTIFIER + "cur";
        public static readonly string VAR_MAX = VARIABLE_IDENTIFIER + "max";
        public static readonly string VAR_MIN = VARIABLE_IDENTIFIER + "min";

        public static string GetFormattedText(GameAttribute attribute, string format)
        {
            //$cur.2 / $max% --> 54.75 / 120%
            // . before the decimals (or to notify the end of the declaration) (can be omitted if no decimals)
            int start = format.IndexOf(VARIABLE_IDENTIFIER);

            while (start != -1)
            {
                string var = format.Substring(start, 4);

                int index = 3;
                bool isDecimal = start + ++index < format.Length && format[start + index] == '.';
                int decimals = isDecimal && char.IsDigit(format[start + ++index]) ? int.Parse(format[start + index++].ToString()) : 0;

                string decimalsStr = "0.";
                for (int i = 0; i < decimals; i++)
                    decimalsStr += "0";

                double value = 0.0;

                if (var == VAR_CURRENT) value = attribute.Current;
                else if (var == VAR_MAX) value = attribute.Max;
                else if (var == VAR_MIN) value = attribute.Min;

                string text = value.ToString(decimalsStr, CultureInfo.InvariantCulture);
                format = format.Replace(format.Substring(start, index), text);

                start = format.IndexOf(VARIABLE_IDENTIFIER);

            }

            return format;
        }
    }
}
