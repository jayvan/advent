using System;

public class Advent01 {
  public static void Main () {
    string numbers = Console.ReadLine();

    Console.WriteLine("Part One: " + SumForOffset(numbers, 1));
    Console.WriteLine("Part Two: " + SumForOffset(numbers, numbers.Length / 2));
  }

  private static double SumForOffset(string input, int offset) {
    double sum = 0;

    for (int i = 1; i < input.Length; i++) {
      int otherIndex = (i + offset) % input.Length;

      if (input[i] == input[otherIndex]) {
        sum += Char.GetNumericValue(input[i]);
      }
    }

    return sum;
  }
}
