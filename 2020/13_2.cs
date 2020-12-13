using System;
using System.Linq;

public class Advent132 {
  public static void Main() {
    Console.ReadLine();
    string[] input = Console.ReadLine().Split(',');
    int[] offsets = new int[input.Count(a => a != "x")];
    int[] indices = new int[offsets.Length];
    int[] sorted = new int[offsets.Length];

    int j = 0;
    for (int i = 0; i < input.Length; i++) {
      if (input[i] == "x") {
        continue;
      }

      offsets[j] = int.Parse(input[i]);
      sorted[j] = offsets[j];
      indices[j] = i;

      j++;
    }

    Array.Sort(sorted);
    int biggest = sorted[sorted.Length - 1];
    int biggestIndex = indices[Array.IndexOf(offsets, biggest)];

    long earliest = biggest - biggestIndex;
//    earliest += (100000000000000 / biggest) * biggest;

    int lowestMultiple = (lcm(sorted[sorted.Length - 1], sorted[sorted.Length - 2]));
    Console.WriteLine($"Busses: {string.Join(",", offsets)}");
    Console.WriteLine($"Indices: {string.Join(",", indices)}");
    Console.WriteLine($"Earliest: {earliest}");
    Console.WriteLine($"Biggest: {biggest}");
    Console.WriteLine($"Biggest Index: {biggestIndex}");
    Console.WriteLine($"Least common multiple: {lowestMultiple}");

    int[] subsetOffsets = new int[2];
    int[] subsetIndices = new int[2];
    subsetOffsets[0] = sorted[sorted.Length - 1];
    subsetOffsets[1] = sorted[sorted.Length - 2];
    subsetIndices[0] = indices[Array.IndexOf(offsets, subsetOffsets[0])];
    subsetIndices[1] = indices[Array.IndexOf(offsets, subsetOffsets[1])];

    earliest = FindTime(earliest, biggest, subsetOffsets, subsetIndices);

    Console.WriteLine(FindTime(earliest, lowestMultiple, offsets, indices));
  }

  public static long FindTime(long earliest, int step, int[] offsets, int[] indices) {
    int runs = 0;
    while (!IsValid(earliest, offsets, indices)) {
      earliest += step;
      runs++;

      if (runs % 1000000 == 0) {
        Console.WriteLine(earliest);
      }
    }

    return earliest;
  }

  public static bool IsValid(long timestamp, int[] offsets, int[] indices) {
    for (int i = 0; i < offsets.Length; i++) {
      if ((timestamp + indices[i]) % offsets[i] != 0) {
        return false;
      }
    }

    return true;
  }

  public static int lcm(int a, int b) {
    return (a * b) / gcd(a, b);
  }

  public static int gcd(int a, int b) {
    int t;

    while (b != 0) {
      t = b;
      b = a % b;
      a = t;
    }

    return a;
  }
}
