﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Text
{
    public class AttributeText
    {
        /// <summary>
        /// Contains the default opening delimiter of the expression ('{' because a expression is by default delimited by '{ ... }'
        /// </summary>
        public const char DEFAULT_OPENING_EXPRESSION_DELIMITER = '{';
        /// <summary>
        /// Contains the default closing delimiter of the expression ('}' because a expression is by default delimited by '{ ... }'
        /// </summary>
        public const char DEFAULT_CLOSING_EXPRESSION_DELIMITER = '}';
        public const char ATTRIBUTE_IDENTIFIER = '$';
        public const string SELF_ATTRIBUTE_IDENTIFIER = "this";

        public const int DEFAULT_ROUND_DECIMALS = 2;

        private string simpleText;

        private string resolvedText = "";
        private bool resolvedTextNeedCache = false;

        private ExpressionText expressionText = new ExpressionText();

        //private List<GameAttribute> dependantAttributes = new List<GameAttribute>();
        public List<GameAttribute> DependantAttributes { get; set; }

        //public IDictionary<string, GameAttribute> DependantAttributes = new Dictionary<string, GameAttribute>();
        public GameAttribute LinkedAttribute { get; set; }

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

        /// <summary>
        /// Contains the opening delimiter of the expression
        /// </summary>
        public char OpeningExpressionDelimiter
        {
            get { return expressionText.OpeningExpressionDelimiter; }
            set { expressionText.OpeningExpressionDelimiter = value; }
        }
        /// <summary>
        /// Contains the closing delimiter of the expression
        /// </summary>
        public char ClosingExpressionDelimiter
        {
            get { return expressionText.ClosingExpressionDelimiter; }
            set { expressionText.ClosingExpressionDelimiter = value; }
        }

        public int RoundDecimals
        {
            get { return expressionText.RoundDecimals; }
            set { expressionText.RoundDecimals = value; }
        }

        public AttributeText (GameAttribute linkedAttribute)
        {
            LinkedAttribute = linkedAttribute;
            DependantAttributes = new List<GameAttribute>();

            RoundDecimals = DEFAULT_ROUND_DECIMALS;
            OpeningExpressionDelimiter = DEFAULT_OPENING_EXPRESSION_DELIMITER;
            ClosingExpressionDelimiter = DEFAULT_CLOSING_EXPRESSION_DELIMITER;
        }

        public bool TryGetResolvedDouble (out double result)
        {
            return double.TryParse(GetResolvedText(), out result);
        }

        public bool TryGetResolvedInteger (out int result)
        {
            return int.TryParse(GetResolvedText(), out result);
        }

        public double GetResolvedDouble ()
        {
            double resolved = 0.0;
            double.TryParse(GetResolvedText(), NumberStyles.Any, CultureInfo.InvariantCulture, out resolved);
            return resolved;
        }

        public int GetResolvedInteger()
        {
            int resolved = 0;
            if (int.TryParse(GetResolvedText(), NumberStyles.Any, CultureInfo.InvariantCulture, out resolved))
            {
                resolved = (int)Math.Round(GetResolvedDouble(), MidpointRounding.AwayFromZero);
            }
            return resolved;
        }

        /*public void AddDependance (GameAttribute attribute)
        {
            dependantAttributes.Add(attribute);
        }

        public void ClearDependances ()
        {
            dependantAttributes.Clear();
        }

        public void RemoveDependance (GameAttribute attribute)
        {
            dependantAttributes.Remove(attribute);
        }*/

        public string GetResolvedText ()
        {
            if (resolvedTextNeedCache)
            {
                resolvedText = SimpleText;


                ResolveAttribute(LinkedAttribute, ATTRIBUTE_IDENTIFIER + SELF_ATTRIBUTE_IDENTIFIER);

                foreach (GameAttribute attrib in DependantAttributes)
                {
                    ResolveAttribute(attrib);
                }

                resolvedTextNeedCache = false;
            }

            expressionText.SimpleText = expressionText.OpeningExpressionDelimiter + resolvedText + expressionText.ClosingExpressionDelimiter;
            return expressionText.GetResolvedText();
        }

        private void ResolveAttribute(GameAttribute attribute, string identifier = "")
        {
            identifier = identifier == "" ? ATTRIBUTE_IDENTIFIER + attribute.Abbreviation.ToLower() : identifier;
            int index;

            //$this.name

            if ((index = resolvedText.IndexOf(identifier)) != -1)
            {
                int end = resolvedText.IndexOf(' ', index);
                int start = resolvedText.IndexOf('.', index, end) + 1;
                end -= start;
                
                string propName = "current";
                string replace = identifier;

                if (start != 0)
                {
                    propName = resolvedText.Substring(start, end);
                    replace = identifier + "." + propName;
                }

                object value = attribute.GetType().GetProperty(propName[0].ToString().ToUpper() + propName.Substring(1)).GetValue(attribute);

                resolvedText = resolvedText.Replace(replace, value.ToString());
            }
        }
    }
}
