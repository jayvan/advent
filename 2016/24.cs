using System;
using System.Collections.Generic;
using System.Text;

public class Advent24 {
  struct State {
    public int X;
    public int Y;
    public bool[] Visited;

    public bool Complete {
      get {
        for (int i = 0; i < Visited.Length; i++) {
          if (!Visited[i] ) {
            return false;
          }
        }

        return true;
      }
    }

    public State(int x, int y, bool[] visited) {
      X = x;
      Y = y;
      Visited = visited;
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder(16);
      builder.Append(X);
      builder.Append('|');
      builder.Append(Y);
      builder.Append('|');

      for (int i = 0; i < Visited.Length; i++) {
        builder.Append(Visited[i] ? '1' : '0');
      }

      return builder.ToString();
    }
  }

  public static void Main () {
    var input = new List<string>();

    string line;

    while ((line = Console.ReadLine()) != null) {
      input.Add(line);
    }

    var grid = new bool[input[0].Length, input.Count];
    var locations = new Dictionary<KeyValuePair<int, int>, int>();
    var start = new KeyValuePair<int, int>();

    for (int y = 0; y < input.Count; y++) {
      string row = input[y];

      for (int x = 0; x < row.Length; x++) {
        if (row[x] == '#') {
          grid[x, y] = false;
        } else if (row[x] == '.') {
          grid[x, y] = true;
        } else {
          grid[x, y] = true;
          var location = new KeyValuePair<int, int>(x, y);
          int number = row[x] - '0';
          locations.Add(location, number);

          if (number == 0) {
            start = location;
          }
        }
      }
    }

    int steps = 1;
    var currentStates = new List<State>();
    var nextStates = new List<State>();
    var visited = new HashSet<string>();
    var initialVisited = new bool[locations.Count];
    initialVisited[0] = true;
    var initState = new State(start.Key, start.Value, initialVisited);
    currentStates.Add(initState);

    while (true) {
      foreach (State state in currentStates) {
        foreach (State nextState in NextStatesFor(state, grid, locations)) {
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

  private static State BuildState(int newX, int newY, State oldState, Dictionary<KeyValuePair<int, int>, int> locations) {
    bool[] visited = oldState.Visited;
    var location = new KeyValuePair<int, int>(newX, newY);
    if (locations.ContainsKey(location)) {
      int wire = locations[location];
      if (!visited[wire]) {
        visited = (bool[])visited.Clone();
        visited[wire] = true;
      }
    }

    return new State(newX, newY, visited);
  }

  private static IEnumerable<State> NextStatesFor(State state, bool[,] grid, Dictionary<KeyValuePair<int, int>, int> locations) {
    // Try moving up
    if (state.Y != 0 && grid[state.X, state.Y - 1]) {
      yield return BuildState(state.X, state.Y - 1, state, locations);
    }

    // Try moving down
    if (state.Y < grid.GetLength(1) - 1 && grid[state.X, state.Y + 1]) {
      yield return BuildState(state.X, state.Y + 1, state, locations);
    }

    // Try moving left
    if (state.X != 0 && grid[state.X - 1, state.Y ]) {
      yield return BuildState(state.X - 1, state.Y, state, locations);
    }

    // Try moving right
    if (state.X < grid.GetLength(0) - 1 && grid[state.X + 1, state.Y ]) {
      yield return BuildState(state.X + 1, state.Y, state, locations);
    }
  }
}
