using System;
using System.Collections.Generic;
using System.Net;

public enum State {
  Empty,
  Taken,
  Floor
}
public class Advent11 {
  private static int[][] directions = {
    new[] {1, 0}, new[] {-1, 0}, new[] {0, 1}, new[] {0, -1},
    new[] {1, 1}, new[] {-1, 1}, new[] {-1, -1}, new[] {1, -1}
  };

  public static void Main() {
    string line;

    List<State[]> floor = new List<State[]>();
    List<State[]> floorTwo = new List<State[]>();

    while ((line = Console.ReadLine()) != null) {
      State[] row = new State[line.Length];

      for (int i = 0; i < line.Length; i++) {
        row[i] = line[i] == 'L' ? State.Empty : State.Floor;
      }

      floor.Add(row);
      floorTwo.Add((State[])row.Clone());
    }


    while (Update(floor, floorTwo)) {
//      Console.WriteLine("---");
      List<State[]> tmp = floor;
      floor = floorTwo;
      floorTwo = tmp;
      //Print(floor);
    }

    int taken = 0;

    for (int y = 0; y < floor.Count; y++) {
      for (int x = 0; x < floor[y].Length; x++) {
        if (floor[y][x] == State.Taken) {
          taken++;
        }
      }
    }

    Console.WriteLine(taken);
  }

  private static bool Update(List<State[]> input, List<State[]> output) {
    bool updateHappened = false;

    for (int y = 0; y < input.Count; y++) {
      for (int x = 0; x < input[y].Length; x++) {
        if (input[y][x] != State.Floor) {
          int surrounding = Surrounding2(input, x, y);
          if (input[y][x] == State.Empty && surrounding == 0) {
            output[y][x] = State.Taken;
            updateHappened = true;
          } else if (input[y][x] == State.Taken && surrounding >= 5) {
            updateHappened = true;
            output[y][x] = State.Empty;
          } else {
            output[y][x] = input[y][x];
          }
        }
      }
    }

    return updateHappened;
  }

  private static void Print(List<State[]> floor) {
    for (int y = 0; y < floor.Count; y++) {
      for (int x = 0; x < floor[y].Length; x++) {
        if (floor[y][x] == State.Taken) {
          Console.Write('#');
        } else if (floor[y][x] == State.Empty) {
          Console.Write('L');
        } else {
          Console.Write('.');
        }
      }

      Console.Write('\n');
    }
  }

  private static int Surrounding2(List<State[]> floor, int startX, int startY) {
    int taken = 0;

    for (int i = 0; i < directions.Length; i++) {
      int dx = directions[i][0];
      int dy = directions[i][1];

      int x = startX + dx;
      int y = startY + dy;

      while (x >= 0 && y >= 0 && x < floor[0].Length && y < floor.Count && floor[y][x] == State.Floor) {
        x += dx;
        y += dy;
      }

      if (x >= 0 && y >= 0 && x < floor[0].Length && y < floor.Count && floor[y][x] == State.Taken) {
        taken++;
      }
    }

    return taken;
  }

  private static int Surrounding(List<State[]> floor, int x, int y) {
    int taken = 0;

    for (int i = x - 1; i <= x + 1; i++) {
      for (int j = y - 1; j <= y + 1; j++) {
        if (j < floor.Count && j >= 0 && i >= 0 && i < floor[0].Length && (j != y || x != i)) {
          if (floor[j][i] == State.Taken) {
            taken++;
          }
        }
      }
    }

    return taken;
  }
}
