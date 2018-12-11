using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Advent08 {
  public static void Main(string[] args) {
    IEnumerable<int> input = File.ReadAllLines("input.txt")[0].Split(' ').Select(a => int.Parse(a));
    IEnumerator<int> iterator = input.GetEnumerator();
    Tree tree = BuildTree(iterator);
    Console.WriteLine(tree.Sum());
  }

  private static Tree BuildTree(IEnumerator<int> iterator) {
    iterator.MoveNext();
    int children = iterator.Current;

    iterator.MoveNext();
    int metadataCount = iterator.Current;

    Tree tree = new Tree(children, metadataCount);

    for (int i = 0; i < children; i++) {
      tree.Children[i] = BuildTree(iterator);
    }

    for (int i = 0; i < metadataCount; i++) {
      iterator.MoveNext();
      tree.Metadata[i] = iterator.Current;
    }

    return tree;
  }
}

public class Tree {
  public Tree[] Children;
  public int[] Metadata;
  private int? cachedSum;

  public Tree(int childCount, int metadataCount) {
    this.Children = new Tree[childCount];
    this.Metadata = new int[metadataCount];
  }

  public int Sum() {
    if (cachedSum.HasValue) {
      return cachedSum.Value;
    }

    int sum = 0;
    if (this.Children.Length == 0) {
      sum = this.Metadata.Sum();
    } else {
      foreach (int i in Metadata) {
        if (i > 0 && i <= this.Children.Length) {
          sum += this.Children[i - 1].Sum();
        }
      }
    }

    this.cachedSum = sum;
    return sum;
  }
}
