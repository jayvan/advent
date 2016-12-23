using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Advent21 {
  struct State {
    public int GoalX;
    public int GoalY;
    public int GapX;
    public int GapY;

    public bool Complete {
      get {
        return GoalX == 0 && GoalY == 0;
      }
    }

    public State(int goalX, int goalY, int gapX, int gapY) {
      GoalX = goalX;
      GoalY = goalY;
      GapX = gapX;
      GapY = gapY;
    }

    public string ToString() {
      StringBuilder builder = new StringBuilder(11);
      builder.Append(GoalX);
      builder.Append('|');
      builder.Append(GoalY);
      builder.Append('|');
      builder.Append(GapX);
      builder.Append('|');
      builder.Append(GapY);
      return builder.ToString();
    }
  }

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
  }

  public static void Main () {
    var nodeRegex = new Regex("/dev/grid/node-x([0-9]+)-y([0-9]+) +([0-9]+)T +([0-9]+)T +([0-9]+)");

    Node[,] grid = new Node[3, 3];

    int gapX = 0;
    int gapY = 0;
    int gapSize = 0;

    string line;

    while ((line = Console.ReadLine()) != null) {
      Match match = nodeRegex.Match(line);

      if (!match.Success) {
        continue;
      }

      int x = int.Parse(match.Groups[1].Value);
      int y = int.Parse(match.Groups[2].Value);
      int size = int.Parse(match.Groups[3].Value);
      int used = int.Parse(match.Groups[4].Value);
      if (used == 0) {
        gapX = x;
        gapY = y;
        gapSize = size;
        Console.WriteLine(x + " " + y);
      }
      var node = new Node(x, y, size, used);
      grid[x, y] = node;
    }

    var blacklist = new HashSet<KeyValuePair<int, int>>();

    for (int x = 0; x < grid.GetLength(0); x++) {
      for (int y = 0; y < grid.GetLength(1); y++) {
        if (grid[x, y].Used > gapSize) {
          blacklist.Add(new KeyValuePair<int, int>(x, y));
          Console.WriteLine("Blacklisted " + x + " " + y);
        }
      }
    }

    int steps = 1;
    var currentStates = new List<State>();
    var nextStates = new List<State>();
    var visited = new HashSet<string>();
    var initState = new State(grid.GetLength(0) - 1, 0, gapX, gapY);
    currentStates.Add(initState);

    while (true) {
      foreach (State state in currentStates) {
        foreach (State nextState in NextStatesFor(state, blacklist, 3, 3)) {
          if (nextState.Complete) {
            Console.WriteLine("Found a valid solution in " + steps + " steps!");
            return;
          }

          string hash = nextState.ToString();

          if (!visited.Contains(hash)) {
            nextStates.Add(nextState);
            visited.Add(hash);
          }
        }
      }

      currentStates = nextStates;
      nextStates = new List<State>();
      steps++;

      Console.WriteLine("Steps taken: " + steps);
      Console.WriteLine("Current states: " + currentStates.Count);
      Console.WriteLine("Visited states: " + visited.Count);
    }
  }

  private static IEnumerable<State> NextStatesFor(State state, HashSet<KeyValuePair<int, int>> blacklist, int width, int height) {
    // Move in from left
    if (state.GapX != 0 && !blacklist.Contains(new KeyValuePair<int, int>(state.GapX - 1, state.GapY))) {
      if (state.GoalX == state.GapX - 1 && state.GoalY == state.GapY) {
        yield return new State(state.GapX, state.GapY, state.GapX - 1, state.GapY);
      } else {
        yield return new State(state.GoalX, state.GoalY, state.GapX - 1, state.GapY);
      }
    }

    // Move in from right
    if (state.GapX != width - 1 && !blacklist.Contains(new KeyValuePair<int, int>(state.GapX + 1, state.GapY))) {
      if (state.GoalX == state.GapX + 1 && state.GoalY == state.GapY) {
        yield return new State(state.GapX, state.GapY, state.GapX + 1, state.GapY);
      } else {
        yield return new State(state.GoalX, state.GoalY, state.GapX + 1, state.GapY);
      }
    }

    // Move in from above
    if (state.GapY != 0 && !blacklist.Contains(new KeyValuePair<int, int>(state.GapX, state.GapY - 1))) {
      if (state.GoalX == state.GapX && state.GoalY == state.GapY - 1) {
        yield return new State(state.GapX, state.GapY, state.GapX, state.GapY - 1);
      } else {
        yield return new State(state.GoalX, state.GoalY, state.GapX, state.GapY - 1);
      }
    }

    // Move in from below
    if (state.GapY != height - 1 && !blacklist.Contains(new KeyValuePair<int, int>(state.GapX, state.GapY + 1))) {
      if (state.GoalX == state.GapX && state.GoalY == state.GapY + 1) {
        yield return new State(state.GapX, state.GapY, state.GapX, state.GapY + 1);
      } else {
        yield return new State(state.GoalX, state.GoalY, state.GapX, state.GapY + 1);
      }
    }
  }
}
