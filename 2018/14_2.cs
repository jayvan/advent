using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

public class Advent14_02 {
  public static void Main() {
    int input = int.Parse(Console.ReadLine());
    int[] inputPieces = input.ToString().ToCharArray().Select(a => a - '0').ToArray();
    int lastIndexChecked = 0;
    var recipes = new List<int>(100000);
    recipes.Add(3);
    recipes.Add(7);

    int[] elves = new int[2];
    for (int i = 0; i < elves.Length; i++) {
      elves[i] = i;
    }

    while (true) {
      int combination = 0;

      for (int i = 0; i < elves.Length; i++) {
        combination += recipes[elves[i]];
      }

      var newRecipes = combination.ToString().ToCharArray().Select(a => int.Parse(a.ToString()));
      recipes.AddRange(newRecipes);

      for (int i = 0; i < elves.Length; i++) {
        elves[i] = (elves[i] + recipes[elves[i]] + 1) % recipes.Count;
      }

      for (; lastIndexChecked < recipes.Count - inputPieces.Length; lastIndexChecked++) {
        bool match = true;
        for (int j = 0; j < inputPieces.Length; j++) {
          if (inputPieces[j] != recipes[lastIndexChecked + j]) {
            match = false;
            break;
          }
        }

        if (match) {
          Console.WriteLine(lastIndexChecked);
          Environment.Exit(0);
        }
      }
    }
  }
}
