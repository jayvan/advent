using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Advent192 {
  public static void Main() {
    var registry = new Dictionary<int, Rule>();
    string line;

    while ((line = Console.ReadLine()) != string.Empty) {
      string[] entry = line.Split(new[] {": "}, StringSplitOptions.None);
      int ruleNumber = int.Parse(entry[0]);

      if (entry[1][0] == '"') {
        registry.Add(ruleNumber, new BasicRule(entry[1][1]));
      } else if (entry[1].IndexOf('|') > 0) {
        string[] sequences = entry[1].Split(new[] {" | "}, StringSplitOptions.None);
        Rule seqA = MakeSequence(registry, sequences[0]);
        Rule seqB = MakeSequence(registry, sequences[1]);
        registry.Add(ruleNumber, new ChoiceRule(ruleNumber, seqA, seqB));
      } else {
        registry.Add(ruleNumber, MakeSequence(registry, entry[1]));
      }
    }

    // Make replacements
    registry[8] = new ChoiceRule(8, MakeSequence(registry, "42"), MakeSequence(registry, "42 8"));
    registry[11] = new ChoiceRule(11, MakeSequence(registry, "42 31"), MakeSequence(registry, "42 11 31"));

    RootRule root = new RootRule(registry);
    string reg = root.Regex();
    Console.WriteLine(reg);
    Regex regex = new Regex(reg);

    int correct = 0;

    while ((line = Console.ReadLine()) != null) {
      if (regex.Match(line).Success) {
        correct++;
      }
    }

    Console.WriteLine(correct);
  }

  private static SequenceRule MakeSequence(Dictionary<int, Rule> registry, string sequence) {
    List<int> seq = sequence.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    return new SequenceRule(registry, seq);
  }
}

public class RootRule {
  private Dictionary<int, Rule> Registry;

  public RootRule(Dictionary<int, Rule> registry) {
    this.Registry = registry;
  }

  public string Regex() {
    return $"^{this.Registry[0].Regex(new Dictionary<int, int>())}$";
  }
}

public class ChoiceRule : Rule {
  private const int MAX_DEPTH = 5;
  private Rule FirstChoice;
  private Rule SecondChoice;
  private int Id;

  public ChoiceRule(int id, Rule a, Rule b) {
    this.Id = id;
    this.FirstChoice = a;
    this.SecondChoice = b;
  }

  public string Regex(Dictionary<int, int> visits) {
    if (!visits.ContainsKey(this.Id)) {
      visits[this.Id] = 0;
    }

    string firstChoice = this.FirstChoice.Regex(visits);

    if (visits[this.Id] >= MAX_DEPTH) {
      return firstChoice;
    }

    visits[this.Id]++;

    string regex = $"({firstChoice}|{this.SecondChoice.Regex(visits)})";

    visits[this.Id]--;

    return regex;
  }
}

public class SequenceRule : Rule {
  private Dictionary<int, Rule> Registry;
  private List<int> Sequence;

  public SequenceRule(Dictionary<int, Rule> registry, List<int> sequence) {
    this.Sequence = sequence;
    this.Registry = registry;
  }

  public string Regex(Dictionary<int, int> visits) {
    string regex = string.Empty;

    foreach (int i in this.Sequence) {
      regex += this.Registry[i].Regex(visits);
    }

    return regex;
  }
}

public class BasicRule : Rule {
  private char Needle;

  public BasicRule(char needle) {
    this.Needle = needle;
  }

  public string Regex(Dictionary<int, int> visits) {
    return this.Needle.ToString();
  }
}

public interface Rule {
  string Regex(Dictionary<int, int> visits);
}
