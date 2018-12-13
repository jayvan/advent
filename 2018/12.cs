using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Advent12 {
  public static void Main() {
    const int PAD = 40;
    var mappings = new Dictionary<string, char>();
    var builder = new StringBuilder(5);
    string line;
    char[] plants = ("........................................" + Console.ReadLine().Split(' ')[2] + "........................................").ToCharArray();
    char[] newPlants = new char[plants.Length];
    for (int i = 0; i < newPlants.Length; i++) {
      newPlants[i] = '.';
    }

    Console.ReadLine();
    while ((line = Console.ReadLine()) != null) {
      string[] pieces = line.Split(new[] { " => " }, StringSplitOptions.None);
      mappings.Add(pieces[0], pieces[1][0]);
    }

    for (int i = 0; i < 20; i++) {
      Console.WriteLine(i + ": " + new String(plants));
      for (int x = 2; x < plants.Length - 3; x++) {
        builder.Clear();

        for (int c = x - 2; c <= x + 2; c++) {
          builder.Append(plants[c]);
        }

        if (mappings.ContainsKey(builder.ToString())) {
          newPlants[x] = mappings[builder.ToString()];
        } else {
          newPlants[x] = '.';
        }
      }

      char[] tmp = plants;
      plants = newPlants;
      newPlants = tmp;
    }

    int total = 0;
    for (int i = 0; i < plants.Length; i++) {
      if (plants[i] == '#') {
        total += i - PAD;
      }
    }

    Console.WriteLine(total);
  }
}

