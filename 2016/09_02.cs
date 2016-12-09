using System;

public class Advent092 {
  public static void Main () {
    string line = Console.ReadLine();
    Console.WriteLine("Length: " + GetLength(line));
  }

  private static long GetLength(string code) {
    long length = 0;

    for (int i = 0; i < code.Length; i++) {
      if (code[i] == '(') {
        int endOfExpansion = code.IndexOf(')', i);
        string expansion = code.Substring(i + 1,  endOfExpansion - i - 1);
        string[] lengthAndReps = expansion.Split(new char[] { 'x' });
        int repLength = int.Parse(lengthAndReps[0]);
        int reps = int.Parse(lengthAndReps[1]);
        string substr = code.Substring(endOfExpansion + 1, repLength);

        length += reps * GetLength(substr);
        i = endOfExpansion + repLength;
      } else {
        length++;
      }
    }

    return length;
  }
}
