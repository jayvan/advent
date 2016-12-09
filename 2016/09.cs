using System;

public class Advent09 {
  public static void Main () {
    string result = "";
    string line = Console.ReadLine();

    for (int i = 0; i < line.Length; i++) {
      if (line[i] == '(') {
        int endOfExpansion = line.IndexOf(')', i);
        string expansion = line.Substring(i + 1,  endOfExpansion - i - 1);
        string[] lengthAndReps = expansion.Split(new char[] { 'x' });
        int length = int.Parse(lengthAndReps[0]);
        int reps = int.Parse(lengthAndReps[1]);

        string substr = line.Substring(endOfExpansion + 1, length);
        for (int j = 0; j < reps; j++) {
          result += substr;
        }
        i = endOfExpansion + length;
      } else {
        result += line[i];
      }
    }

    Console.WriteLine("Length: " + result.Length);
  }
}
