using System;
using System.Text.RegularExpressions;

public class Advent20 {
  public class Vector3 {
    private int X;
    private int Y;
    private int Z;

    public int Distance {
      get {
        return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
      }
    }

    public Vector3(int x, int y, int z) {
      X = x;
      Y = y;
      Z = z;
    }

    public void Add(Vector3 other) {
      this.X += other.X;
      this.Y += other.Y;
      this.Z += other.Z;
    }
  }

  public class Particle {
    private Vector3 Position;
    private Vector3 Velocity;
    private Vector3 Acceleration;
    private readonly Regex regex = new Regex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>");

    public Particle(string line) {
      Match match = regex.Match(line);
      Position = new Vector3(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
      Velocity = new Vector3(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value));
      Acceleration = new Vector3(int.Parse(match.Groups[7].Value), int.Parse(match.Groups[8].Value), int.Parse(match.Groups[9].Value));
    }

    public void Update() {
      Velocity.Add(Acceleration);
      Position.Add(Velocity);
    }

    public int Distance {
      get {
        return Position.Distance;
      }
    }
  }

  public static void Main() {
    string line;
    int smallestDistance = int.MaxValue;
    int closest = -1;
    int particleNum = 0;

    while ((line = Console.ReadLine()) != null) {
      Particle p = new Particle(line);
      for (int i = 0; i < 1000; i++) {
        p.Update();
      }

      if (p.Distance < smallestDistance) {
        smallestDistance = p.Distance;
        closest = particleNum;
      }

      particleNum++;
    }

    Console.WriteLine(closest);
  }
}
