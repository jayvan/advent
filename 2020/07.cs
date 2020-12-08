using System;
using System.Collections.Generic;

public class Advent07 {
  private static string[] lineSplit = {" bags contain "};
  private static string[] bagSplit = {" bag, ", " bags, ", " bag.", " bags."};
  private static string SHINY_GOLD = "shiny gold";
  private static Dictionary<string,Dictionary<string,int>> bagNest = new Dictionary<string,Dictionary<string,int>>();
  private static Dictionary<string, int> bagCache = new Dictionary<string, int>();

  public static void Main() {
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] pieces = line.Split(lineSplit, StringSplitOptions.None);
      string source = pieces[0];

      if (pieces[1] == "no other bags.") {
        continue;
      }

      var outputs = new Dictionary<string, int>();

      foreach (string target in pieces[1].Split(bagSplit, StringSplitOptions.RemoveEmptyEntries)) {
        int count = int.Parse(target.Substring(0, 1));
        string bag = target.Substring(2);

        outputs.Add(bag, count);
      }

      bagNest.Add(source, outputs);
    }

    int goldCount = -1;

    foreach (var keyValuePair in bagNest) {
      if (HasGold(keyValuePair.Key)) {
        goldCount++;
      }
    }

    Console.WriteLine(goldCount);

    Console.WriteLine(BagCount(SHINY_GOLD) - 1);
  }

  private static int BagCount(string bag) {
    // Memoize
    if (bagCache.ContainsKey(bag)) {
      return bagCache[bag];
    }

    // Base case, bag has no more bags nested, it's solo
    if (!bagNest.ContainsKey(bag)) {
      bagCache.Add(bag, 1);
      return 1;
    }

    int count = 1;

    foreach (var keyValuePair in bagNest[bag]) {
      count += BagCount(keyValuePair.Key) * keyValuePair.Value;
    }

    bagCache.Add(bag, count);
    return count;
  }

  private static bool HasGold(string bag) {
    if (bag == SHINY_GOLD) {
      return true;
    }

    if (!bagNest.ContainsKey(bag)) {
      return false;
    }

    foreach (var keyValuePair in bagNest[bag]) {
      if (HasGold(keyValuePair.Key)) {
        return true;
      }
    }

    return false;
  }
}
