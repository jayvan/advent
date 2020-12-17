using System;
using System.Collections.Generic;
using System.Linq;

public class Advent16 {
  public static void Main() {
    string line;
    TicketSystem system = new TicketSystem();
    var departureIndices = new List<int>();

    // 1. Rules
    while ((line = Console.ReadLine()) != string.Empty) {
      string[] parts = line.Split(new[] {": ", " or "}, StringSplitOptions.RemoveEmptyEntries);
      var ranges = new List<KeyValuePair<int, int>>();

      for (int i = 1; i < parts.Length; i++) {
        string[] partParts = parts[i].Split('-');
        ranges.Add(new KeyValuePair<int, int>(int.Parse(partParts[0]), int.Parse(partParts[1])));
      }

      system.AddRule(ranges);
    }

    // 2. My ticket
    Console.ReadLine();
    string[] myTicket = Console.ReadLine().Split(',');
    Console.ReadLine();
    Console.ReadLine();

    int errorRate = 0;

    // 3. Nearby Tickets
    while ((line = Console.ReadLine()) != null) {
      string[] fields = line.Split(',');
      int[] ticket = new int[fields.Length];

      for (int index = 0; index < fields.Length; index++) {
        string field = fields[index];
        int num = int.Parse(field);
        ticket[index] = num;
      }

      errorRate += system.AddTicket(ticket);
    }

    Console.WriteLine("---");
    Console.WriteLine(errorRate);

    // Go through each ticket and record if each field is valid for each index to identify guarantees
    // Then repeat, ignoring fields we've determined until it's resolved
    system.Build();

    ulong ticketNum = 1;
    Console.WriteLine(myTicket.Length);
    for (int i = 0; i < 6; i++) {
      ticketNum *= ulong.Parse(myTicket[system.fieldsForRules[i].First()]);
    }
    Console.WriteLine(ticketNum);
  }

}

public class TicketSystem {
  private List<List<KeyValuePair<int, int>>> rules = new List<List<KeyValuePair<int, int>>>();
  private List<int[]> validTickets = new List<int[]>();
  public List<HashSet<int>> fieldsForRules = new List<HashSet<int>>();

  public void AddRule(List<KeyValuePair<int, int>> rule) {
    this.rules.Add(rule);
  }

  public int AddTicket(int[] ticket) {
    // Check each field of the ticket and ensure it can pass a rule
    for (int i = 0; i < ticket.Length; i++) {
      bool validField = false;

      foreach (List<KeyValuePair<int, int>> rule in this.rules) {
        if (FieldPassesRule(ticket[i], rule)) {
          validField = true;
          break;
        }
      }

      if (!validField) {
        return ticket[i];
      }
    }

    this.validTickets.Add(ticket);
    return 0;
  }

  private bool FieldPassesRule(int field, List<KeyValuePair<int, int>> rule) {
    bool passes = false;

    foreach (KeyValuePair<int, int> keyValuePair in rule) {
      if (field >= keyValuePair.Key && field <= keyValuePair.Value) {
        passes = true;
        break;
      }
    }

    if (!passes) {
      //Console.WriteLine($"Value {field} doesn't match {rules.IndexOf(rule)}");
    }

    return passes;
  }

  public void Build() {
    // For each rule, determine which field indices could work
    Console.WriteLine("Build stats\n-----");
    for (int ruleIndex = 0; ruleIndex < this.rules.Count; ruleIndex++) {
      List<KeyValuePair<int, int>> rule = this.rules[ruleIndex];
      HashSet<int> possibleFields = new HashSet<int>();

      for (int fieldIndex = 0; fieldIndex < this.rules.Count; fieldIndex++) {
        bool allTicketsValid = this.validTickets.All(ticket => FieldPassesRule(ticket[fieldIndex], rule));

        if (allTicketsValid) {
          possibleFields.Add(fieldIndex);
        }
      }

      this.fieldsForRules.Add(possibleFields);
      Console.WriteLine($"Rule {ruleIndex} has {possibleFields.Count} choices: {string.Join(",", possibleFields)}");
    }

    for (int i = 0; i < this.fieldsForRules.Count; i++) {
      if (this.fieldsForRules[i].Count == 1) {
        Cleanup(this.fieldsForRules[i].First(), i);
      }
    }

    for (int i = 0; i < this.fieldsForRules.Count; i++) {
      Console.WriteLine($"{i}: {this.fieldsForRules[i].First()}");
    }
  }

  private void Cleanup(int value, int index) {
    Console.WriteLine($"Value {value} must be in index {index}");
    for (int i = 0; i < this.fieldsForRules.Count; i++) {
      if (i != index) {
        var removed = this.fieldsForRules[i].Remove(value);

        if (removed && this.fieldsForRules[i].Count == 1) {
          Cleanup( this.fieldsForRules[i].First(), i);
        }
      }
    }
  }
}

