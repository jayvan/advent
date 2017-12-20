using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent20 {
  public struct Vector3 {
    private int X;
    private int Y;
    private int Z;

    public int Distance {
      get {
        return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
      }
    }

    public override string ToString() {
      return $"<{X}, {Y}, {Z}>";
    }

    public Vector3(int x, int y, int z) {
      X = x;
      Y = y;
      Z = z;
    }

    public Vector3 Add(Vector3 other) {
      return new Vector3(this.X + other.X, this.Y + other.Y, this.Z + other.Z);
    }
  }

  public class Particle {
    public Vector3 Position { get; private set; }
    private Vector3 Velocity;
    private Vector3 Acceleration;
    private readonly Regex regex = new Regex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>");
    public readonly int Number;

    public Particle(string line, int num) {
      Number = num;
      Match match = regex.Match(line);
      Position = new Vector3(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value));
      Velocity = new Vector3(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), int.Parse(match.Groups[6].Value));
      Acceleration = new Vector3(int.Parse(match.Groups[7].Value), int.Parse(match.Groups[8].Value), int.Parse(match.Groups[9].Value));
    }

    public void Update() {
      Velocity = Velocity.Add(Acceleration);
      Position = Position.Add(Velocity);
    }

    public int Distance {
      get {
        return Position.Distance;
      }
    }
  }

  public static void Main() {
    string line;
    var particles = new List<Particle>();

    while ((line = Console.ReadLine()) != null) {
      Particle p = new Particle(line, particles.Count);
      particles.Add(p);
    }

    for (int i = 0; i < 10000; i++) {
      var originals = new Dictionary<Vector3, int>();
      var others = new List<int>();

      for (int j = particles.Count - 1; j >= 0; j--) {
        particles[j].Update();
        if (originals.ContainsKey(particles[j].Position)) {
          if (!others.Contains(originals[particles[j].Position])) {
            others.Add(originals[particles[j].Position]);
          }

          others.Add(j);
        } else {
          originals.Add(particles[j].Position, j);
        }
      }

      foreach (int idx in others) {
        particles.RemoveAt(idx);
      }
    }


    Console.WriteLine(particles.Count);
  }
}
