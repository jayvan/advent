using System;
using System.Collections.Generic;

public class Advent12 {
  public static void Main () {
    int favouriteNumber = int.Parse(Console.ReadLine());
    var destination = new KeyValuePair<int, int>(31, 39);
    var initialState = new KeyValuePair<int, int>(1, 1);
    int steps = 1;
    var currentStates = new List<KeyValuePair<int, int>>();
    var nextStates = new List<KeyValuePair<int, int>>();
    var visited = new HashSet<KeyValuePair<int, int>>();
    currentStates.Add(initialState);

    while (true) {
      foreach (KeyValuePair<int, int> state in currentStates) {
        foreach (KeyValuePair<int, int> nextState in AdjacentStates(state, favouriteNumber)) {
          if (nextState.Equals(destination)) {
            Console.WriteLine("Found a valid solution in " + steps + " steps!");
            return;
          }

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
