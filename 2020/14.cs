using System;
using System.Collections.Generic;

public class Advent14 {
  public static void Main() {
    string line;
    ulong andMask = 0;
    ulong orMask = 0;
    var registers = new Dictionary<int, ulong>();

    while ((line = Console.ReadLine()) != null) {
      string[] input = line.Split(new[] {" = "}, StringSplitOptions.None);
      string lhs = input[0];
      string rhs = input[1];

      if (lhs == "mask") {
        andMask = ulong.MaxValue;
        orMask = ulong.MinValue;

        for (int i = 0; i < rhs.Length; i++) {
          if (rhs[i] == '1') {
            orMask += (ulong)1 << (rhs.Length - i - 1);
          } else if (rhs[i] == '0') {
            andMask -= (ulong)1 << (rhs.Length - i - 1);
          }
        }
      } else {
        int register = int.Parse(lhs.Split(new[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries)[1]);
        registers[register] = ulong.Parse(rhs) & andMask | orMask;
        Console.WriteLine($"Wrote {registers[register]} to {register}");
      }
    }

    ulong sum = 0;

    foreach (KeyValuePair<int, ulong> keyValuePair in registers) {
      sum += keyValuePair.Value;
    }

    Console.WriteLine(sum);
  }
}
