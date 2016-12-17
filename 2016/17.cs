using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class Advent17 {

  struct State {
    public int x;
    public int y;
    public string code;

    public State(int x, int y, string code) {
      this.x = x;
      this.y = y;
      this.code = code;
    }

    public bool IsDestination {
      get {
        return x == 3 && y == 3;
      }
    }

    public IEnumerable<State> Adjacent {
      get {
        using (MD5 md5 = MD5.Create()) {
          byte[] hashInput = Encoding.ASCII.GetBytes(code);
          string hash = BitConverter.ToString(md5.ComputeHash(hashInput)).Replace("-","");

          // Try going up
          if (y > 0 && hash[0] > 'A') {
            yield return new State(x, y - 1, code + "U");
          }

          // Try going down
          if (y < 3 && hash[1] > 'A') {
            yield return new State(x, y + 1, code + "D");
          }

          // Try going left
          if (x > 0 && hash[2] > 'A') {
            yield return new State(x - 1, y, code + "L");
          }

          // Try going right
          if (x < 3 && hash[3] > 'A') {
            yield return new State(x + 1, y, code + "R");
          }
        }
      }
    }

  }

  public static void Main () {
    string passcode = Console.ReadLine();
    var initialState = new State(0, 0, passcode);
    int steps = 1;
    var currentStates = new List<State>();
    var nextStates = new List<State>();
    var visited = new HashSet<string>();
    currentStates.Add(initialState);

    while (true) {
      foreach (State state in currentStates) {
        foreach (State nextState in state.Adjacent) {
          if (nextState.IsDestination) {
            Console.WriteLine($"Found a solution: {nextState.code.Substring(passcode.Length)}");
            return;
          }

          if (!visited.Contains(nextState.code)) {
            nextStates.Add(nextState);
            visited.Add(nextState.code);
          }
        }
      }

      currentStates = nextStates;
      nextStates = new List<State>();
      steps++;
    }
  }
}
