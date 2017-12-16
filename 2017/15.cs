using System;

public class Advent14 {
  private class Generator {
    private int pastValue;
    private readonly long factor;

    public Generator(long factor, int initial) {
      pastValue = initial;
      this.factor = factor;
    }

    public int Next() {
      pastValue = (int)((pastValue * factor) % 2147483647);
      return pastValue;
    }
  }

  public static void Main() {
    var a = new Generator(16807, int.Parse(Console.ReadLine()));
    var b = new Generator(48271, int.Parse(Console.ReadLine()));

    int count = 0;

    for (int i = 0; i < 40000000; i++) {
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
