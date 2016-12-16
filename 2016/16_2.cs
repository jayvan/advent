using System;

public class Advent16 {
  public static void Main () {
    char[] a = Console.ReadLine().ToCharArray();

    while (a.Length < 35651584) {
      char[] b = (char[])a.Clone();

      for (int i = 0; i < a.Length / 2; i++) {
        char tmp = b[i];
        b[i] = b[b.Length - 1 - i];
        b[b.Length - 1 - i] = tmp;
      }

      for (int i = 0; i < b.Length; i++) {
        b[i] = b[i] == '0' ? '1' : '0';
      }

      var c = new char[a.Length * 2 + 1];
      a.CopyTo(c, 0);
      c[a.Length] = '0';
      b.CopyTo(c, a.Length + 1);
      a = c;
    }

    string disk = new string(a, 0, 35651584);
    char[] hash;

    do {
      hash = new char[disk.Length / 2];

      for (int i = 0; i < disk.Length; i += 2) {
        hash[i / 2] = disk[i] == disk[i + 1] ? '1' : '0';
      }

      disk = new string(hash);
    } while (hash.Length % 2 == 0);

    Console.WriteLine(hash);
  }
}
