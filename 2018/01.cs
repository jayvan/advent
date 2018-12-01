using System;

public class Advent01 {
  public static void Main () {
    int frequency = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      frequency += int.Parse(line);
    }

    Console.WriteLine("Frequency: " + frequency);
  }
}
