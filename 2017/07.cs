using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Advent06 {
  class Tower {
    Dictionary<string, string[]> children = new Dictionary<string, string[]>();
    Dictionary<string, string> parent = new Dictionary<string, string>();
    Dictionary<string, int> individualWeights = new Dictionary<string, int>();
    Dictionary<string, int> cumulativeWeights = new Dictionary<string, int>();

    public string TopProgram {
      get {
        string currentProgram = parent.Keys.First();

        while (parent.ContainsKey(currentProgram)) {
          currentProgram = parent[currentProgram];
        }

        return currentProgram;
      }
    }

    public int FindBadProgramDesiredWeight() {
      string badParent = FindBadParent();

      // Find all siblings, if we match with one of them then our weight is the correct one
      // If we don't match with any, we need to match their weight
      string[] siblings = children[badParent];
      var siblingWeights = children[badParent].Select(sibling => CumulativeWeight(sibling)).ToArray();

      int properWeight = siblingWeights[0];

      if (Array.LastIndexOf(siblingWeights, siblingWeights[0]) == 0) {
        properWeight = siblingWeights[1];
      }

      int indexOfAbnormality = Array.FindIndex(siblingWeights, siblingWeight => siblingWeight != properWeight);
      return properWeight - siblingWeights[indexOfAbnormality] + individualWeights[children[badParent][indexOfAbnormality]];
    }

    public string FindBadParent() {
      // From the root, descend into the node that is imbalanced
      // Once we reach a node that is balanced, we've found the offender

      string program = TopProgram;

      while (!ProgramBalanced(program)) {
        bool foundNext = false;
        foreach (string child in children[program]) {
          if (!ProgramBalanced(child)) {
            program = child;
            foundNext = true;
            break;
          }
        }

        if (!foundNext) {
          return program;
        }
      }

      return program;
    }

    public bool ProgramBalanced(string program) {
      if (children[program].Length == 0) {
        return true;
      }

      int childWeight = CumulativeWeight(children[program][0]);

      for (int i = 1; i < children[program].Length; i++) {
        if (CumulativeWeight(children[program][i]) != childWeight) {
          return false;
        }
      }

      return true;
    }

    public int CumulativeWeight(string program) {
      if (cumulativeWeights.ContainsKey(program)) {
        return cumulativeWeights[program];
      }

      int cumulativeWeight = individualWeights[program];

      foreach (string child in children[program]) {
        cumulativeWeight += CumulativeWeight(child);
      }

      cumulativeWeights.Add(program, cumulativeWeight);

      return cumulativeWeight;
    }

    public void AddProgram(string name, int weight, string[] programChildren) {
      if (programChildren != null) {
        foreach (string childName in programChildren) {
          parent.Add(childName, name);
        }
      }

      children.Add(name, programChildren);

      individualWeights.Add(name, weight);
    }
  }

  public static void Main () {
    var regex = new Regex(@"(\w+) \((\d+)\)( -> (.*))?");
    string[] childDelimeter = new[] { ", " };
    var tower = new Tower();

    string line;
    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);
      string program = match.Groups[1].Value;
      int weight = int.Parse(match.Groups[2].Value);
      string[] children = null;

      if (match.Groups[3].Success) {
        children = match.Groups[4].Value.Split(childDelimeter, StringSplitOptions.RemoveEmptyEntries);
      } else {
        children = new string[0];
      }

      tower.AddProgram(program, weight, children);
    }

    Console.WriteLine($"Top level program: {tower.TopProgram}");
    Console.WriteLine($"Bad program needs a weight of {tower.FindBadProgramDesiredWeight()}");
  }
}
