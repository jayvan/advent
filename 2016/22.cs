using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent21 {
  struct Node {
    public int X;
    public int Y;
    public int Size;
    public int Used;

    public Node(int x, int y, int size, int used) {
      X = x;
      Y = y;
      Size = size;
      Used = used;
    }

    public int Available {
      get {
        return Size - Used;
      }
    }

    public override string ToString() {
      return $"({X}, {Y}) - Used: {Used} Free: {Available}";
    }
  }

  class NodeUsedComp : IComparer<Node> {
    public int Compare(Node a, Node b) {
      if (a.Used.CompareTo(b.Used) != 0) {
        return a.Used.CompareTo(b.Used);
      }

      if (a.X.CompareTo(b.X) != 0) {
        return a.X.CompareTo(b.X);
      }

      return a.Y.CompareTo(b.Y);
    }
  }

  class NodeFreeComp : IComparer<Node> {
    public int Compare(Node a, Node b) {
      if (a.Available.CompareTo(b.Available) != 0) {
        return a.Available.CompareTo(b.Available);
      }

      if (a.X.CompareTo(b.X) != 0) {
        return a.X.CompareTo(b.X);
      }

      return a.Y.CompareTo(b.Y);
    }
  }

  public static void Main () {
    var nodeRegex = new Regex("/dev/grid/node-x([0-9]+)-y([0-9]+) +([0-9]+)T +([0-9]+)T +([0-9]+)");

    var nodesByUsed = new SortedSet<Node>(new NodeUsedComp());
    var nodesByFree = new SortedSet<Node>(new NodeFreeComp());

    string line;

    int pairs = 0;

    while ((line = Console.ReadLine()) != null) {
      Match match = nodeRegex.Match(line);

      if (!match.Success) {
        continue;
      }

      int x = int.Parse(match.Groups[1].Value);
      int y = int.Parse(match.Groups[2].Value);
      int size = int.Parse(match.Groups[3].Value);
      int used = int.Parse(match.Groups[4].Value);
      var node = new Node(x, y, size, used);

      // A pair is any two nodes (A, B) such that..
      // A is not empty (its Used is not zero).
      // The used of A is <= the free of B

      // Find existing nodes for which this can be an A
      if (node.Used > 0) {
        Node bLower = new Node(0, 0, node.Used, 0);
        Node bHigher = new Node(0, 0, int.MaxValue, 0);
        pairs += nodesByFree.GetViewBetween(bLower, bHigher).Count;
      }

      // Find existing nodes for which this can be a B
      Node aLower = new Node(0, 0, 0, 1);
      Node aHigher = new Node(0, 0, 0, node.Available);
      pairs += nodesByUsed.GetViewBetween(aLower, aHigher).Count;

      nodesByUsed.Add(node);
      nodesByFree.Add(node);
    }

    Console.WriteLine(pairs);
  }
}
