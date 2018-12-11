using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Advent08 {
  public static void Main(string[] args) {
    IEnumerable<int> input = File.ReadAllLines("input.txt")[0].Split(' ').Select(a => int.Parse(a));
    IEnumerator<int> iterator = input.GetEnumerator();
    Console.WriteLine(SumTree(iterator));
  }

  private static int SumTree(IEnumerator<int> iterator) {
    int sum = 0;
    iterator.MoveNext();
    int children = iterator.Current;

    iterator.MoveNext();
    int metadataCount = iterator.Current;

    for (int i = 0; i < children; i++) {
      sum += SumTree(iterator);
    }

    for (int i = 0; i < metadataCount; i++) {
      iterator.MoveNext();
      sum += iterator.Current;
    }

    return sum;
  }
}
