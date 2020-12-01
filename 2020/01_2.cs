using System;

public class Advent01 {
  public static void Main () {
    int[] numbers = new int[200];

    for (int i = 0; i < 200; i++) {
      numbers[i] = int.Parse(Console.ReadLine());
    }

    for (int a = 0; a < 200; a++) {
      for (int b = a + 1; b < 200; b++) {
        for (int c = b + 1; c < 200; c++) {
          if (numbers[a] + numbers[b] + numbers[c] == 2020) {
            Console.WriteLine(numbers[a] * numbers[b] * numbers[c]);
          }
        }
      }
    }
  }
}
