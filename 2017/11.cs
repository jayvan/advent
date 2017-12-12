using System;
using System.Collections.Generic;

public class Advent11 {
  private struct HexLocation {
    readonly int X;
    readonly int Y;
    readonly int Z;

    public HexLocation(int x, int y, int z) {
      X = x;
      Y = y;
      Z = z;
    }

    public static HexLocation operator +(HexLocation v1, HexLocation v2) {
      return new HexLocation(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }

    public int DistanceFromOrigin {
      get {
        return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) / 2;
      }
    }
  }
  public static void Main () {
    var mappings = new Dictionary<string, HexLocation> {
      { "se", new HexLocation(1, -1,  0) },
      { "ne" , new HexLocation(1,  0, -1) },
      { "n", new HexLocation( 0, 1, -1) },
      { "nw", new HexLocation(-1, 1,  0) },
      { "sw", new HexLocation(-1,  0, 1) },
      { "s", new HexLocation( 0, -1, 1) }
    };

    string line;

    while ((line = Console.ReadLine()) != null) {
      var position = new HexLocation();
      int furthestDistance = 0;

      foreach (string dir in line.Split(',')) {
        position += mappings[dir];
        furthestDistance = Math.Max(furthestDistance, position.DistanceFromOrigin);
      }

      Console.WriteLine($"Distance from origin: {position.DistanceFromOrigin}");
      Console.WriteLine($"Furthest distance from origin: {furthestDistance}");
    }
  }
}
