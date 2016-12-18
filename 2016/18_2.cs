using System;

public class Advent18 {
  public static void Main () {
    const int NUM_ROWS = 400000;

    int safeCount = 0;
    string initial = Console.ReadLine();

    var currentFloor = new bool[initial.Length];

    for (int i = 0; i < currentFloor.Length; i++) {
      if (initial[i] == '^') {
        currentFloor[i] = true;
      } else {
        safeCount++;
      }
    }

    var floors = new bool[NUM_ROWS][];
    floors[0] = currentFloor;

    for (int i = 1; i < NUM_ROWS; i++) {
      bool[] previous = floors[i - 1];
      var newFloor = new bool[previous.Length];
      floors[i] = newFloor;

      for (int j = 0; j < newFloor.Length; j++) {
        bool left = j > 0 && previous[j - 1];
        bool center = previous[j];
        bool right= j < newFloor.Length - 1 && previous[j + 1];

        newFloor[j] = left && center && !right || !left && center && right || left && !center && !right || !left && !center && right;

        if (!newFloor[j]) {
          safeCount++;
        }
      }
    }

    Console.WriteLine($"Number of safe spots: {safeCount}");
  }
}
