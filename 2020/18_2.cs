using System;
using System.Text.RegularExpressions;

public class Advent18_2 {
  static Regex justNumber = new Regex(@"^\d+$");
  private static char[] ops = {'+', '*', '('};

  public static void Main() {
    string line;
    ulong sum = 0;

    while ((line = Console.ReadLine()) != null) {
      Console.WriteLine($"{line} = {Eval(line)}");
      sum += Eval(line);
    }

    Console.WriteLine(sum);
  }

  private static ulong Eval(string expr) {
//    Console.WriteLine($"Evaluating: '{expr}'");
    // base case, there's just a number
    if (justNumber.Match(expr).Success) {
      return ulong.Parse(expr);
    }


    // 1. Whittle away parentheses
    while (expr.Contains("(")) {
      int start = expr.IndexOf('(');
      int end = start + 1;
      int depth = 1;

      for (; end < expr.Length; end++) {
        if (expr[end] == '(') {
          depth++;
        } else if (expr[end] == ')') {
          depth--;

          if (depth == 0) {
            break;
          }
        }
      }

      string sub = expr.Substring(start + 1, end - start - 1);
      string repl = Eval(sub).ToString();
//      Console.WriteLine($"Substituting '{sub}' -> '{repl}'");
      expr = expr.Replace('(' + sub + ')', Eval(sub).ToString());
    }

    // 2. Split on * which leaves + expressions
    string[] subExprs = expr.Split(new[] {" * "}, StringSplitOptions.None);

    ulong result = 1;
    foreach (string subExpr in subExprs) {
      ulong sum = 0;

      foreach (string s in subExpr.Split(new[] {" + "}, StringSplitOptions.None)) {
        sum += ulong.Parse(s);
      }

      result *= sum;
    }

    return result;
  }
}
