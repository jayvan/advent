using System;
using System.Collections.Generic;
using System.Linq;

public class Advent15 {
  public static void Main() {
    HashSet<Tuple<int, int>> walls = new HashSet<Tuple<int, int>>();
    List<Unit> units = new List<Unit>();
    int width = 0;

    string line;
    int height = 0;
    while ((line = Console.ReadLine()) != null) {
      width = line.Length;
      for (int x = 0; x < line.Length; x++) {
        if (line[x] == '#') {
          walls.Add(new Tuple<int, int>(x, height));
        } else if (line[x] != '.') {
          units.Add(new Unit(line[x] == 'E', x, height));
        }
      }

      height++;
    }

    World world = new World(width, height, walls, units);

    while (!world.GameOver) {
      world.Update();
    }

    int totalHp = units.Select(a => a.Health).Sum();

    Console.WriteLine($"Battle commenced after {world.Steps} rounds");
    Console.WriteLine($"Remaining HP: {totalHp}");
    Console.WriteLine($"Answer: {totalHp * world.Steps}");
  }
}

public class World {
  private List<Unit> units;
  private bool[,] grid;

  public int Steps {
    get {
      return this.units.Select(u => u.Turns).Min() - 1;
    }
  }

  public bool GameOver {
    get {
      return this.units.All(unit => unit.IsElf) || this.units.All(unit => !unit.IsElf);
    }
  }

  public World(int width, int height, HashSet<Tuple<int, int>> walls, List<Unit> units) {
    this.grid = new bool[width, height];
    this.units = units;

    for (int y = 0; y < this.grid.GetLength(1); y++) {
      for (int x = 0; x < this.grid.GetLength(0); x++) {
        var location = new Tuple<int, int>(x, y);
        if (walls.Contains(location)) {
          grid[x, y] = false;
        } else {
          grid[x, y] = this.units.Find(u => u.X == x && u.Y == y) == null;
        }
      }
    }
  }

  public void Update() {
    // Units go in top-down-left-to-right order. let's sort the list to get them ordered
    this.units.Sort();

    foreach (Unit unit in this.units) {
      if (unit.Health <= 0) {
        continue;
      }
      unit.DoTurn(this.grid, this.units);
    }

    this.units.RemoveAll(unit => unit.Health <= 0);
  }

  public void Print() {
    for (int y = 0; y < this.grid.GetLength(1); y++) {
      for (int x = 0; x < this.grid.GetLength(0); x++) {
        if (grid[x, y]) {
          Console.Write('.');
        } else {
          var unit = this.units.Find(u => u.X == x && u.Y == y);
          if (unit != null) {
            Console.Write(unit.IsElf ? 'E' : 'G');
          } else {
            Console.Write('#');
          }
        }
      }
      Console.Write('\n');
    }
  }
}

public class Unit : IComparable {
  public int Turns = 0;
  public readonly bool IsElf;
  public int X { get; private set; }
  public int Y { get; private set; }

  public Unit(bool elf, int x, int y) {
    this.IsElf = elf;
    this.X = x;
    this.Y = y;
    this.Health = 200;
  }

  public IEnumerable<Tuple<int, int>> Adjacent() {
    yield return new Tuple<int, int>(X, Y - 1);
    yield return new Tuple<int, int>(X - 1, Y);
    yield return new Tuple<int, int>(X + 1, Y);
    yield return new Tuple<int, int>(X, Y + 1);
  }

  public void MoveDestination(bool[,] open, List<Unit> units) {
    // Check if we are already adjacent to an enemy, if so we don't move
    foreach (Tuple<int, int> tuple in this.Adjacent()) {
      if (open[tuple.Item1, tuple.Item2]) {
        continue;
      }

      Unit unit = units.Find(u => u.X == tuple.Item1 && u.Y == tuple.Item2);
      if (unit != null && unit.IsElf != this.IsElf) {
        return;
      }
    }

    // Identify all locations adjacent to enemies
    var targets = new List<Tuple<int, int>>();

    foreach (Unit unit in units) {
      if (unit.Health >= 1 && unit.IsElf != this.IsElf) {
        foreach (Tuple<int, int> tuple in unit.Adjacent()) {
          if (open[tuple.Item1, tuple.Item2]) {
            targets.Add(tuple);
          }
        }
      }
    }

    if (targets.Count == 0) {
      return;
    }

    List<Tuple<int, int, int>> reachable = Reachable(X, Y, open, targets);
    if (reachable.Count == 0) {
      return;
    }

    // Find closest of the reachable locations, tie breaker is lowest y then lowest x
    reachable.Sort(this.TupleSort);
    Tuple<int, int, int> destination = reachable[0];

    var newLocation = NextDestination(open, new Tuple<int, int>(destination.Item1, destination.Item2));
    open[this.X, this.Y] = true;
    this.X = newLocation.Item1;
    this.Y = newLocation.Item2;
    open[this.X, this.Y] = false;

  }
  private Tuple<int, int> NextDestination(bool[,] open, Tuple<int, int> target) {
    var targetList = new List<Tuple<int, int>> {target};
    // Try moving up, then left, right, down and see which has the shortest distance
    var toTry = new List<Tuple<int,int>>(this.Adjacent().Where(loc => open[loc.Item1, loc.Item2]));

    var best = toTry[0];
    var bestDistance = int.MaxValue;

    foreach (Tuple<int, int> tuple in toTry) {
      int distance = 0;
      if (tuple.Item1 != target.Item1 || tuple.Item2 != target.Item2) {
        var result = Reachable(tuple.Item1, tuple.Item2, open, targetList);

        if (result.Count == 0) {
          continue;
        }
        distance = result[0].Item3;
      }

      if (distance < bestDistance) {
        bestDistance = distance;
        best = tuple;
      }
    }

    return best;
  }

  private List<Tuple<int, int, int>> Reachable(int x, int y, bool[,] open, List<Tuple<int, int>> locations) {
    var visited = new HashSet<Tuple<int, int>>();
    var reachable = new List<Tuple<int, int, int>>();
    var visitList = new List<Tuple<int, int>>();
    var newVisitList = new List<Tuple<int, int>>();
    int distance = 1;

    // Breadth first search
    visitList.Add(new Tuple<int, int>(x + 1, y));
    visitList.Add(new Tuple<int, int>(x - 1, y));
    visitList.Add(new Tuple<int, int>(x, y - 1));
    visitList.Add(new Tuple<int, int>(x, y + 1));

    while (visitList.Count != 0) {
      foreach (Tuple<int, int> tuple in visitList) {
        if (!open[tuple.Item1, tuple.Item2]) {
          continue;
        }

        if (visited.Contains(tuple)) {
          continue;
        }

        visited.Add(tuple);

        if (locations.Contains(tuple)) {
          reachable.Add(new Tuple<int, int, int>(tuple.Item1, tuple.Item2, distance));
          if (reachable.Count == locations.Count) {
            return reachable;
          }
        }

        newVisitList.Add(new Tuple<int, int>(tuple.Item1 + 1, tuple.Item2));
        newVisitList.Add(new Tuple<int, int>(tuple.Item1 - 1, tuple.Item2));
        newVisitList.Add(new Tuple<int, int>(tuple.Item1, tuple.Item2 + 1));
        newVisitList.Add(new Tuple<int, int>(tuple.Item1, tuple.Item2 - 1));
      }

      distance++;
      List<Tuple<int, int>> tmp = visitList;
      visitList = newVisitList;
      newVisitList = tmp;
      newVisitList.Clear();
    }

    return reachable;
  }

  public int CompareTo(object obj) {
    Unit other = obj as Unit;

    int comparison = this.Y.CompareTo(other.Y);

    if (comparison == 0) {
      comparison = this.X.CompareTo(other.X);
    }

    return comparison;
  }

  public void DoTurn(bool[,] grid, List<Unit> units) {
    MoveDestination(grid, units);
    AttackNeighbour(grid, units);
    Turns++;
  }

  private void AttackNeighbour(bool[,] grid, List<Unit> units) {
    int lowestHp = int.MaxValue;
    Unit toHit = null;

    foreach (Tuple<int, int> tuple in this.Adjacent()) {
      if (grid[tuple.Item1, tuple.Item2]) {
        continue;
      }

      Unit unit = units.Find(u => u.X == tuple.Item1 && u.Y == tuple.Item2);
      if (unit != null && unit.IsElf != this.IsElf && unit.Health < lowestHp) {
        lowestHp = unit.Health;
        toHit = unit;
      }
    }

    toHit?.Hit(grid);
  }

  private void Hit(bool[,] grid) {
    this.Health -= 3;
    if (this.Health <= 0) {
      grid[this.X, this.Y] = true;
      this.X = -1;
      this.Y = -1;
    }
  }

  public int Health { get; set; }

  private int TupleSort(Tuple<int, int> a, Tuple<int, int> b) {
    int comparison = a.Item2.CompareTo(b.Item2);

    if (comparison == 0) {
      comparison = a.Item1.CompareTo(b.Item1);
    }

    return comparison;
  }

  private int TupleSort(Tuple<int, int, int> a, Tuple<int, int, int> b) {
    int comparison = a.Item3.CompareTo(b.Item3);

    if (comparison == 0) {
      comparison = a.Item2.CompareTo(b.Item2);
    }

    if (comparison == 0) {
      comparison = a.Item1.CompareTo(b.Item1);
    }

    return comparison;
  }
}
