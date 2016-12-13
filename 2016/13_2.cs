using System;
using System.Collections.Generic;

public class Advent12 {
  public static void Main () {
    int favouriteNumber = int.Parse(Console.ReadLine());
    var initialState = new KeyValuePair<int, int>(1, 1);
    int steps = 0;
    var currentStates = new List<KeyValuePair<int, int>>();
    var nextStates = new List<KeyValuePair<int, int>>();
    var visited = new HashSet<KeyValuePair<int, int>>();
    currentStates.Add(initialState);

    while (steps < 50) {
      foreach (KeyValuePair<int, int> state in currentStates) {
        foreach (KeyValuePair<int, int> nextState in AdjacentStates(state, favouriteNumber)) {
          if (!visited.Contains(nextState)) {
            nextStates.Add(nextState);
            visited.Add(nextState);
          }
        }
      }

      currentStates = nextStates;
      nextStates = new List<KeyValuePair<int, int>>();
      steps++;
    }

    Console.WriteLine($"Visisted {visited.Count} locations");
  }

  private static IEnumerable<KeyValuePair<int, int>> AdjacentStates(KeyValuePair<int, int> state, int favouriteNumber) {

    // Try going left
    if (state.Key > 0 && IsOpen(state.Key - 1, state.Value, favouriteNumber)) {
      yield return new KeyValuePair<int, int>(state.Key - 1, state.Value);
    }

    // Try going right
    if (IsOpen(state.Key + 1, state.Value, favouriteNumber)) {
      yield return new KeyValuePair<int, int>(state.Key + 1, state.Value);
    }

    // Try going up
    if (state.Value > 0 && IsOpen(state.Key, state.Value - 1, favouriteNumber)) {
      yield return new KeyValuePair<int, int>(state.Key, state.Value - 1);
    }

    // Try going down
    if (IsOpen(state.Key, state.Value + 1, favouriteNumber)) {
      yield return new KeyValuePair<int, int>(state.Key, state.Value + 1);
    }
  }

  private static bool IsOpen(int x, int y, int favouriteNumber) {
    return BitsOn(x*x + 3*x + 2*x*y + y + y*y + favouriteNumber) % 2 == 0;
  }

  private static int BitsOn(int number) {
    int bitsOn = 0;

    while (number > 0) {
      if (number % 2 != 0) {
        bitsOn++;
      }

      number /= 2;
    }

    return bitsOn;
  }
}
