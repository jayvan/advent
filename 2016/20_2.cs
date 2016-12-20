using System;
using System.Collections.Generic;

public class Advent202 {
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
    uint validCount = 0;
    const uint MAX = 4294967295;

    string line;
    while ((line = Console.ReadLine()) != null) {
      string[] splits = line.Split(new[] {'-'});
      ranges.Add(new Range(uint.Parse(splits[0]), uint.Parse(splits[1])));
    }

    uint lowest = 1;

    foreach (Range range in ranges) {
      if (lowest < range.Min) {
        validCount += range.Min - lowest;
      }

      if (range.Max > lowest) {
        lowest = range.Max + 1;

        if (lowest == 0) {
          break;
        }
      }
    }

    if (lowest != 0) {
      validCount += MAX - lowest + 1;
    }

    Console.WriteLine("IP Count: " + validCount);
  }
}
