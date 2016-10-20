using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Engine.Utils
{
    /// <summary>
    /// A math expression is an object reprensentation of a real mathematic expression and will alow the user to evaluate it
    /// </summary>
    public class MathExpression
    {
        #region Constants
        private const string OPERATOR_ADDITION = "+";
        private const string OPERATOR_SUBSTRACTION = "-";
        private const string OPERATOR_MULTIPLICATION = "*";
        private const string OPERATOR_DIVISION = "/";
        private const string OPERATOR_MODULO = "%";

        private const string OPENING_BRACKET = "(";
        private const string CLOSING_BRACKET = ")";

        private const int LEFT_ASSOC = 0;
        private const int RIGHT_ASSOC = 1;
        #endregion

        #region Static Fields
        private static readonly IDictionary<string, int[]> OPERATORS = new Dictionary<string, int[]>();
        private static readonly char[] SEPARATORS = new char[] { OPERATOR_MULTIPLICATION[0], OPERATOR_DIVISION[0], OPERATOR_MODULO[0], OPERATOR_ADDITION[0], OPERATOR_SUBSTRACTION[0], OPENING_BRACKET[0], CLOSING_BRACKET[0] };
        #endregion

        #region Static Constructor
        static MathExpression()
        {
            // Map<"token", []{precendence, associativity}>  
            OPERATORS.Add(OPERATOR_ADDITION, new int[] { 0, LEFT_ASSOC });
            OPERATORS.Add(OPERATOR_SUBSTRACTION, new int[] { 0, LEFT_ASSOC });
            OPERATORS.Add(OPERATOR_MULTIPLICATION, new int[] { 5, LEFT_ASSOC });
            OPERATORS.Add(OPERATOR_DIVISION, new int[] { 5, LEFT_ASSOC });
            OPERATORS.Add(OPERATOR_MODULO, new int[] { 5, LEFT_ASSOC });
        }
        #endregion

        #region Static Helper Methods
        private static string[] tokensToRPN(ref string[] tokens)
        {
            IList<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();

            foreach (string token in tokens)
            {
                if (isOperator(token))
                {
                    while (stack.Count != 0 && isOperator(stack.Peek()))
                    {
                        if ((isAssociative(token, LEFT_ASSOC) && cmpPrecedence(token, stack.Peek()) <= 0) ||
                            (isAssociative(token, RIGHT_ASSOC) && cmpPrecedence(token, stack.Peek()) < 0))
                        {
                            output.Add(stack.Pop());
                            continue;
                        }
                        break;
                    }

                    // Push the new operator on the stack  
                    stack.Push(token);
                }


                // If token is a left bracket '('  
                else if (token == OPENING_BRACKET)
                {
                    stack.Push(token);  //   
                }
                // If token is a right bracket ')'  
                else if (token == CLOSING_BRACKET)
                {
                    while (stack.Count != 0 && stack.Peek() != OPENING_BRACKET)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                // If token is a number  
                else
                {
                    output.Add(token);
                }
            }

            while (stack.Count != 0)
            {
                output.Add(stack.Pop());
            }

            return output.ToArray();
        }

        private static double RPNToDouble(ref string[] tokens)
        {
            Stack<string> stack = new Stack<string>();

            // For each token   
            foreach (String token in tokens)
            {
                // If the token is a value push it onto the stack  
                if (!isOperator(token))
                {
                    stack.Push(token);
                }
                else
                {
                    // Token is an operator: pop top two entries  
                    double d2 = double.Parse(stack.Pop(), CultureInfo.InvariantCulture);
                    double d1 = double.Parse(stack.Pop(), CultureInfo.InvariantCulture);

                    double result = token == OPERATOR_ADDITION ? d1 + d2 :
                                    token == OPERATOR_SUBSTRACTION ? d1 - d2 :
                                    token == OPERATOR_MULTIPLICATION ? d1 * d2 :
                                    token == OPERATOR_DIVISION ? d1 / d2 :
                                    d1 % d2;


                    // Push result onto stack  
                    stack.Push(result.ToString(CultureInfo.InvariantCulture));
                }
            }

            return double.Parse(stack.Pop(), CultureInfo.InvariantCulture);
        }

        private static bool isOperator(string token)
        {
            return OPERATORS.ContainsKey(token);
        }

        private static string cut(ref string str, int count)
        {
            string cut = str.Substring(0, count);
            str = str.Remove(0, count);
            return cut;
        }

        private static bool isAssociative(String token, int type)
        {
            if (!isOperator(token))
            {
                throw new ArgumentException("Invalid token: " + token);
            }

            if (OPERATORS[token][1] == type)
            {
                return true;
            }
            return false;
        }

        private static int cmpPrecedence(String token1, String token2)
        {
            if (!isOperator(token1) || !isOperator(token2))
            {
                throw new ArgumentException("Invalid tokens: " + token1 + " " + token2);
            }
            return OPERATORS[token1][0] - OPERATORS[token2][0];
        }
        #endregion

        #region Properties
        private string expression;
        /// <summary>
        /// Contains the expression as a string
        /// </summary>
        public string Expression
        {
            get { return expression; }
            set
            {
                if (value != expression)
                {
                    expression = value;
                    resultNeedCache = true;
                }
            }
        }

        public bool NeedReEvaluate
        {
            get { return resultNeedCache; }
        }

        #endregion

        #region Fields
        private double cachedResult = 0;
        private bool resultNeedCache = true;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new mathematic expression
        /// </summary>
        public MathExpression ()
        {
            Expression = "";
        }

        /// <summary>
        /// Create a new mathematic expression with the expression string set
        /// </summary>
        /// <param name="expression">The string containing the expression</param>
        public MathExpression (string expression)
        {
            Expression = expression;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Evaluate and return the result of the expression, the result is cached so do not worry about calling this multiple time
        /// </summary>
        /// <returns>The result of the evaluation of the expression</returns>
        public double Evaluate ()
        {
            if (resultNeedCache)
            {

                string[] tokens = getTokens();
                tokens = tokensToRPN(ref tokens);
                cachedResult = RPNToDouble(ref tokens);
                resultNeedCache = false;
            }

            return cachedResult;
        }

        /// <summary>
        /// Evaluate and round the result of the expression to then return it, the result is cached so do not worry about calling this multiple time
        /// </summary>
        /// <returns>The rounded result of the evaluation of the expression</returns>
        public int EvaluateRound ()
        {
            return (int)Evaluate();
        }
        #endregion

        #region Helper Methods
        private string[] getTokens()
        {
            IList<string> tokens = new List<string>();

            string exp = Expression.Trim();
            exp = Regex.Replace(exp, @"\s+", "");

            int index;
            while ((index = exp.IndexOfAny(SEPARATORS)) != -1)
            {
                if (index < 1)
                    index = 1;

                tokens.Add(cut(ref exp, index));
            }

            if (exp.Length != 0)
                tokens.Add(cut(ref exp, exp.Length));

            return tokens.ToArray();
        }
        #endregion
    }
}
