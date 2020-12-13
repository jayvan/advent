using System;

public class Advent13 {
  public static void Main() {
    int earliest = int.Parse(Console.ReadLine());
    int lowestWait = int.MaxValue;
    int lowestBus = -1;


    foreach (string s in Console.ReadLine().Split(',')) {
      if (s != "x") {
        int bus = int.Parse(s);

        int wait = (int)Math.Ceiling((float) earliest / bus) * bus - earliest;
        if (wait < lowestWait) {
          lowestWait = wait;
          lowestBus = bus;
        }
      }
    }

    Console.WriteLine(lowestWait * lowestBus);
  }
}
