using System;
using System.Collections.Generic;
using System.Linq;

public class Advent14 {
  public static void Main() {
    int iterations = int.Parse(Console.ReadLine());
    var recipes = new List<int>(iterations + 10);
    recipes.Add(3);
    recipes.Add(7);

    int[] elves = new int[2];
    for (int i = 0; i < elves.Length; i++) {
      elves[i] = i;
    }

    while (recipes.Count < iterations + 10) {
      int combination = 0;

      for (int i = 0; i < elves.Length; i++) {
        combination += recipes[elves[i]];
      }

      var newRecipes = combination.ToString().ToCharArray().Select(a => int.Parse(a.ToString()));
      recipes.AddRange(newRecipes);

      for (int i = 0; i < elves.Length; i++) {
        elves[i] = (elves[i] + recipes[elves[i]] + 1) % recipes.Count;
      }
    }

    for (int i = iterations; i < iterations + 10; i++) {
      Console.Write(recipes[i]);
    }

    Console.Write("\n");
  }
}
