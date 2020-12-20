using System;
using System.Collections.Generic;
using System.Linq;

namespace nineteen {
  public class Advent19 {
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
          registry.Add(ruleNumber, new ChoiceRule(seqA, seqB));
        } else {
          registry.Add(ruleNumber, MakeSequence(registry, entry[1]));
        }
      }

      RootRule root = new RootRule(registry);

      int correct = 0;

      while ((line = Console.ReadLine()) != null) {
        if (root.Matches(line)) {
          correct++;
        }
      }

      Console.WriteLine(correct);
    }

    private static SequenceRule MakeSequence(Dictionary<int, Rule> registry, string sequence) {
      IEnumerable<int> seq = sequence.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
      return new SequenceRule(registry, seq);
    }
  }

  public class RootRule {
    private Dictionary<int, Rule> Registry;

    public RootRule(Dictionary<int, Rule> registry) {
      this.Registry = registry;
    }

    public bool Matches(string source) {
      int offset = 0;
      return Matches(source, ref offset);
    }

    public bool Matches(string source, ref int offset) {
      return this.Registry[0].Matches(source, true, ref offset);
    }
  }

  public class ChoiceRule : Rule {
    private Rule FirstChoice;
    private Rule SecondChoice;

    public ChoiceRule(Rule a, Rule b) {
      this.FirstChoice = a;
      this.SecondChoice = b;
    }

    public bool Matches(string source, bool last, ref int offset) {
      int firstOffset = offset;

      if (this.FirstChoice.Matches(source, last, ref firstOffset)) {
        offset = firstOffset;
        return true;
      }

      int secondOffset = offset;

      if (this.SecondChoice.Matches(source, last, ref secondOffset)) {
        offset = secondOffset;
        return true;
      }

      return false;
    }
  }

  public class SequenceRule : Rule {
    private Dictionary<int, Rule> Registry;
    private IEnumerable<int> Sequence;

    public SequenceRule(Dictionary<int, Rule> registry, IEnumerable<int> sequence) {
      this.Sequence = sequence;
      this.Registry = registry;
    }

    public bool Matches(string source, bool last, ref int offset) {
      int lastRule = this.Sequence.Last();

      foreach (int i in this.Sequence) {
        if (!this.Registry[i].Matches(source, last && i == lastRule, ref offset)) {
          return false;
        }
      }

      return true;
    }
  }

  public class BasicRule : Rule {
    private char Needle;

    public BasicRule(char needle) {
      this.Needle = needle;
    }

    public bool Matches(string source, bool last, ref int offset) {
      if (offset >= source.Length) {
        return false;
      }

      if (source[offset] == this.Needle) {
        if (last && offset != source.Length - 1) {
          return false;
        } else if (!last && offset == source.Length - 1) {
          return false;
        }
        offset++;
        return true;
      }

      return false;
    }
  }

  public interface Rule {
    bool Matches(string source, bool last, ref int offset);
  }
}
