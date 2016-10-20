using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Engine.Text
{
    /// <summary>
    /// An expression text is a text containing a mathematical expression needed to be evaluated
    /// </summary>
    public class ExpressionText
    {
        /// <summary>
        /// Contains the default opening delimiter of the expression ('{' because a expression is by default delimited by '{ ... }'
        /// </summary>
        public const char DEFAULT_OPENING_EXPRESSION_DELIMITER = '{';
        /// <summary>
        /// Contains the default closing delimiter of the expression ('}' because a expression is by default delimited by '{ ... }'
        /// </summary>
        public const char DEFAULT_CLOSING_EXPRESSION_DELIMITER = '}';

        public int DEFAULT_ROUND_DECIMALS = 2;

        private string simpleText;

        /// <summary>
        /// Contains the unprocessed text with the expression unevaluated
        /// </summary>
        public string SimpleText
        {
            get { return simpleText; }
            set
            {
                if (value != simpleText)
                {
                    simpleText = value;
                    resolvedTextNeedCache = true;
                }
            }
        }

        private string cachedResolvedText;
        private bool resolvedTextNeedCache = true;
        private int roundDecimals;
        private string formatStr;

        /// <summary>
        /// Contains the opening delimiter of the expression
        /// </summary>
        public char OpeningExpressionDelimiter { get; set; }
        /// <summary>
        /// Contains the closing delimiter of the expression
        /// </summary>
        public char ClosingExpressionDelimiter { get; set; }

        public int RoundDecimals
        {
            get { return roundDecimals; }
            set
            {
                roundDecimals = value;
                formatStr = "0.";
                for (int i = 0; i < roundDecimals; i++)
                    formatStr += "0";
            }
        }

        /// <summary>
        /// Create a new expression text
        /// </summary>
        public ExpressionText ()
        {
            SimpleText = "";

            RoundDecimals = DEFAULT_ROUND_DECIMALS;

            OpeningExpressionDelimiter = DEFAULT_OPENING_EXPRESSION_DELIMITER;
            ClosingExpressionDelimiter = DEFAULT_CLOSING_EXPRESSION_DELIMITER;
        }

        /// <summary>
        /// Create a new expression text with the base text already set
        /// </summary>
        /// <param name="simpleText">The unprocessed text with the expression unevaluated</param>
        public ExpressionText (string simpleText)
        {
            SimpleText = simpleText;

            RoundDecimals = DEFAULT_ROUND_DECIMALS;

            OpeningExpressionDelimiter = DEFAULT_OPENING_EXPRESSION_DELIMITER;
            ClosingExpressionDelimiter = DEFAULT_CLOSING_EXPRESSION_DELIMITER;
        }

        /// <summary>
        /// Gets the resolved text where the expression have been evaluated, the resolved text is cached so do not worry about calling this multiple times
        /// </summary>
        /// <returns>The resolved text where the expression have been evaluated</returns>
        public string GetResolvedText ()
        {

            if (resolvedTextNeedCache)
            {
                int start = SimpleText.IndexOf(OpeningExpressionDelimiter);
                int end = SimpleText.IndexOf(ClosingExpressionDelimiter);

                if (start == -1)
                {
                    return SimpleText;
                }

                double result = new MathExpression(SimpleText.Substring(start + 1, end - start - 1)).Evaluate();

                cachedResolvedText = SimpleText.Replace(SimpleText.Substring(start, end - start + 1), result.ToString(formatStr, CultureInfo.InvariantCulture));
            }

            return cachedResolvedText;
        }
    }
}
