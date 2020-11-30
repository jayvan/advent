using System;
using System.Collections.Generic;
using System.Linq;

public class Advent18 {
  private enum State {
    Open,
    Tree,
    Yard
  }

  public static void Main() {
    int SIZE = 50;
    State[,] grid = new State[SIZE, SIZE];
    State[,] scratchGrid = new State[SIZE, SIZE];

    for (int y = 0; y < SIZE; y++) {
      string line = Console.ReadLine();

      for (int x = 0; x < SIZE; x++) {
        if (line[x] == '.') {
          grid[x, y] = State.Open;
        } else if (line[x] == '|') {
          grid[x, y] = State.Tree;
        } else {
          grid[x, y] = State.Yard;
        }
      }
    }

    for (int i = 0; i < 1000000000; i++) {
      int trees = 0;
      int yards = 0;

      for (int y = 0; y < SIZE; y++) {
        for (int x = 0; x < SIZE; x++) {
          if (grid[x, y] == State.Open) {
            // An open acre will become filled with trees if
            // three or more adjacent acres contained trees.

            int treeCount = Adjacent(grid, x, y).Count(a => a == State.Tree);
            scratchGrid[x, y] = treeCount >= 3 ? State.Tree : State.Open;
          } else if (grid[x, y] == State.Tree) {
            // An acre filled with trees will become a lumberyard if
            // three or more adjacent acres were lumberyards.

            int yardCount = Adjacent(grid, x, y).Count(a => a == State.Yard);
            scratchGrid[x, y] = yardCount >= 3 ? State.Yard : State.Tree;
          } else {
            // An acre containing a lumberyard will remain a lumberyard if
            // it was adjacent to at least one other lumberyard and
            // at least one acre containing trees.

            int treeCount = Adjacent(grid, x, y).Count(a => a == State.Tree);
            int yardCount = Adjacent(grid, x, y).Count(a => a == State.Yard);

            scratchGrid[x, y] = yardCount > 0 && treeCount > 0 ? State.Yard : State.Open;
          }

          if (scratchGrid[x, y] == State.Tree) {
            trees++;
          } else if (scratchGrid[x, y] == State.Yard) {
            yards++;
          }
        }
      }

      Console.WriteLine($"{i},{trees * yards}");
      var tmp = grid;
      grid = scratchGrid;
      scratchGrid = tmp;
    }

  }

  private static IEnumerable<State> Adjacent(State[,] grid, int x, int y) {
    for (int xd = x - 1; xd <= x + 1; xd++) {
      for (int yd = y - 1; yd <= y + 1; yd++) {
        if ((xd != x || yd != y) && xd >= 0 && yd >= 0 && xd < grid.GetLength(0) && yd < grid.GetLength(1)) {
          yield return grid[xd, yd];
        }
      }
    }
  }
}
