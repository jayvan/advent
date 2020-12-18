using System;
using System.Text.RegularExpressions;

public class Advent18 {
  static Regex justNumber = new Regex(@"^\d+$");
  private static char[] ops = {'+', '*', ')'};

  public static void Main() {
    string line;
    ulong sum = 0;

    while ((line = Console.ReadLine()) != null) {
      sum += Eval(line);
    }

    Console.WriteLine(sum);
  }

  private static ulong Eval(string expr) {
//    Console.WriteLine($"'{expr}'");
    // base case, there's just a number
    if (justNumber.Match(expr).Success) {
      return ulong.Parse(expr);
    }

    // If we're parenthesized we need to find our closing parenthese and work from there
    // (1 + 2 + 3) * 2
    // LHS = (1 + 2 + 3)
    // Starting search ^
    ulong rhs = 0;
    int startIndex = expr.Length - 2;

    if (expr[expr.Length - 1] == ')') {
      int depth = 1;

      for (; startIndex >=0; startIndex--) {
        if (expr[startIndex] == ')') {
          depth++;
        } else if (expr[startIndex] == '(') {
          depth--;
          if (depth == 0) {
            break;
          }
        }
      }

//      Console.WriteLine($"StartIndex: {startIndex}");
      startIndex = Math.Max(startIndex, 0);
      rhs = Eval(expr.Substring(startIndex + 1, expr.Length - startIndex - 2));
    }

    int opIndex = expr.LastIndexOfAny(ops, startIndex);
    if (expr[expr.Length - 1] != ')') {
      rhs = ulong.Parse(expr.Substring(opIndex + 1));
    }

    if (opIndex < 0) {
      return rhs;
    }

    switch (expr[opIndex]) {
      case '+':
        return rhs + Eval(expr.Substring(0, opIndex - 1));
      case '*':
        return rhs * Eval(expr.Substring(0, opIndex - 1));
      default:
        throw new Exception("Unknown operator " + expr[opIndex]);
    }
  }
}
