using System;

public class Advent14 {
  private class Generator {
    private int pastValue;
    private readonly long factor;
    private readonly long divisor;

    public Generator(long factor, int divisor, int initial) {
      pastValue = initial;
      this.factor = factor;
      this.divisor = divisor;
    }

    public int Next() {
      do {
        pastValue = (int)((pastValue * factor) % 2147483647);
      } while (pastValue % divisor != 0);

      return pastValue;
    }
  }

  public static void Main() {
    var a = new Generator(16807, 4, int.Parse(Console.ReadLine()));
    var b = new Generator(48271, 8, int.Parse(Console.ReadLine()));

    int count = 0;

    for (int i = 0; i < 5000000; i++) {
      if ((a.Next() & 0xFFFF) == (b.Next() & 0xFFFF)) {
        count++;
      }
    }
    Console.WriteLine(count);
  }

  static int BitCount(uint value)
  {
    value = value - ((value >> 1) & 0x55555555);                    // reuse input as temporary
    value = (value & 0x33333333) + ((value >> 2) & 0x33333333);     // temp
    value = ((value + (value >> 4) & 0xF0F0F0F) * 0x1010101) >> 24; // count
    return unchecked((int)value);
  }
}
