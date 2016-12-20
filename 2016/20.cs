using System;
using System.Collections.Generic;

public class Advent20 {
  struct Range {
    public readonly uint Min;
    public readonly uint Max;

    public Range(uint min, uint max) {
      Min = min;
      Max = max;
    }
  }

  class RangeComp : IComparer<Range> {
    public int Compare(Range a, Range b) {
      return a.Min.CompareTo(b.Min) != 0 ? a.Min.CompareTo(b.Min): a.Max.CompareTo(b.Max);
    }
  }

  public static void Main () {
    var ranges = new SortedSet<Range>(new RangeComp());

    string line;
    while ((line = Console.ReadLine()) != null) {
      string[] splits = line.Split(new[] {'-'});
      ranges.Add(new Range(uint.Parse(splits[0]), uint.Parse(splits[1])));
    }

    uint lowest = 1;

    foreach (Range range in ranges) {
      if (range.Min > lowest) {
        break;
      }

      lowest = range.Max + 1;
    }

    Console.WriteLine("Lowest ip possible is: " + lowest);
  }
}
