using System;
using System.Collections.Generic;

public class Advent142 {
  static Dictionary<ulong, ulong> registers = new Dictionary<ulong, ulong>();
  static List<int> wilds = new List<int>();

  public static void Main() {
    string line;
    ulong orMask = 0;

    while ((line = Console.ReadLine()) != null) {
      string[] input = line.Split(new[] {" = "}, StringSplitOptions.None);
      string lhs = input[0];
      string rhs = input[1];

      if (lhs == "mask") {
        wilds.Clear();
        orMask = ulong.MinValue;

        for (int i = 0; i < rhs.Length; i++) {
          if (rhs[i] == '1') {
            orMask += (ulong)1 << (rhs.Length - i - 1);
          } else if (rhs[i] == 'X') {
            wilds.Add(rhs.Length - i - 1);
          }
        }
      } else {
        ulong register = ulong.Parse(lhs.Split(new[] {'[', ']'}, StringSplitOptions.RemoveEmptyEntries)[1]);
        ulong value = ulong.Parse(rhs);
        Write(0, register | orMask, value);
      }
    }

    ulong sum = 0;

    foreach (KeyValuePair<ulong, ulong> keyValuePair in registers) {
      sum += keyValuePair.Value;
    }

    Console.WriteLine(sum);
  }

  private static void Write(int maskIndex, ulong register, ulong value) {
    // base case
    if (maskIndex == wilds.Count) {
      registers[register] = value;
      Console.WriteLine($"Wrote {value} to {register}");
      return;
    }

    // Branch with next bit set and next bit unset

    // 1
    Write(maskIndex + 1, register | ((ulong)1 << wilds[maskIndex]), value);

    // 0
    Write(maskIndex + 1, register & (ulong.MaxValue - ((ulong)1 << wilds[maskIndex])), value);
  }
}
