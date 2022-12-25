using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CalCul
{
	public static class Calculator
	{

		private static readonly char[] Operators = { '+', '-', '*', '/', '(', ')' };
		private static readonly Dictionary<char, int> Precedence = new Dictionary<char, int>
		{
				{ '+', 1 },
				{ '-', 1 },
				{ '*', 2 },
				{ '/', 2 },
				{ '(', 0 }
		};

		internal static string CheckExp(string expression)
		{
			bool flagMul = false;
			bool flagEnd = false;
			StringBuilder currentExpression = new StringBuilder();
			foreach (char c in expression)
			{
				if (char.IsWhiteSpace(c))
					continue;
				if (flagMul && c == '(')
					currentExpression.Append('*');
				if (flagEnd && (char.IsDigit(c) || c == ',' ))
					throw new ArgumentException("Wrong expression");
				flagMul = char.IsDigit(c) || c == ',';
				flagEnd = c == ')';
				currentExpression.Append(c);
			}
			string totalString = Regex.Replace(currentExpression.ToString(), @"-\d+(\,\d+)?", m => "(0" + m.Value + ")");
			totalString = Regex.Replace(totalString, @"\)\(", m => ")+(");
			totalString = Regex.Replace(totalString, @"\d\(0-\d+(\,\d+)?\)", m => m.Value[0] + "+" + m.Value.Substring(1));
			return totalString;
		}

		internal static List<string> Tokenize(string expression)
		{
			List<string> tokens = new List<string>();
			Stack<char> operatorStack = new Stack<char>();
			StringBuilder currentToken = new StringBuilder();

			foreach (char c in expression)
			{
				if (char.IsDigit(c) || c == ',')
					currentToken.Append(c);
				else if (c == '(')
					operatorStack.Push(c);
				else if (c == ')')
				{
					if (currentToken.Length > 0)
					{
						tokens.Add(currentToken.ToString());
						currentToken.Clear();
					}

					while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
						tokens.Add(operatorStack.Pop().ToString());

					if (operatorStack.Count > 0 && operatorStack.Peek() == '(')
						operatorStack.Pop();
					else
						throw new ArgumentException("Mismatched parentheses in expression");
				}
				else if (Array.IndexOf(Operators, c) >= 0)
				{
					if (currentToken.Length > 0)
					{
						tokens.Add(currentToken.ToString());
						currentToken.Clear();
					}

					while (operatorStack.Count > 0 && Precedence[c] <= Precedence[operatorStack.Peek()])
						tokens.Add(operatorStack.Pop().ToString());

					operatorStack.Push(c);
				}
				else
					throw new ArgumentException("Invalid character in expression: " + c);
			}

			if (currentToken.Length > 0)
				tokens.Add(currentToken.ToString());

			while (operatorStack.Count > 0)
			{
				char op = operatorStack.Pop();
				if (op == '(' || op == ')')
					throw new ArgumentException("Mismatched parentheses in expression");
				tokens.Add(op.ToString());
			}
			
			return tokens;
		}



		internal static decimal Evaluate(List<string> tokens)
		{
			Stack<decimal> stack = new Stack<decimal>();

			foreach (string token in tokens)
			{
				if (decimal.TryParse(token, out decimal number))
					stack.Push(number);
				else
				{
					decimal operand2 = stack.Pop();
					decimal operand1 = stack.Pop();

					decimal result;
					switch (token)
					{
						case "+":
							result = operand1 + operand2;
							break;
						case "-":
							result = operand1 - operand2;
							break;
						case "*":
							result = operand1 * operand2;
							break;
						case "/":
							if (operand2 == 0)
								throw new ArgumentException("Divided by zero");
							result = operand1 / operand2;
							break;
						default:
							throw new ArgumentException("Invalid operator: " + token);
					}

					stack.Push(result);
				}
			}

			return stack.Pop();
		}

		public static decimal Compute(string expression)
		{
				return Evaluate(Tokenize(CheckExp(expression)));
		}
	}
}
