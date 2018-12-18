using System;
using System.Collections.Generic;
using System.Linq;

public class Advent16 {
  public static void Main() {
    var commands = new Dictionary<string, Command>();
    commands.Add("addr", new Command(true, true, (a, b) => a + b));
    commands.Add("addi", new Command(true, false, (a, b) => a + b));
    commands.Add("mulr", new Command(true, true, (a, b) => a * b));
    commands.Add("muli", new Command(true, false, (a, b) => a * b));
    commands.Add("banr", new Command(true, true, (a, b) => a & b));
    commands.Add("bani", new Command(true, false, (a, b) => a & b));
    commands.Add("borr", new Command(true, true, (a, b) => a | b));
    commands.Add("bori", new Command(true, false, (a, b) => a | b));
    commands.Add("setr", new Command(true, true, (a, b) => a));
    commands.Add("seti", new Command(false, false, (a, b) => a));
    commands.Add("gtir", new Command(false, true, (a, b) => a > b ? 1 : 0));
    commands.Add("gtri", new Command(true, false, (a, b) => a > b ? 1 : 0));
    commands.Add("gtrr", new Command(true, true, (a, b) => a > b ? 1 : 0));
    commands.Add("eqir", new Command(false, true, (a, b) => a == b ? 1 : 0));
    commands.Add("eqri", new Command(true, false, (a, b) => a == b ? 1 : 0));
    commands.Add("eqrr", new Command(true, true, (a, b) => a == b ? 1 : 0));

    string line;
    int triples = 0;

    while ((line = Console.ReadLine()) != string.Empty) {
      int[] registers = line.Substring(9, line.Length - 10).Split(new[] {", "}, StringSplitOptions.None).Select(a => int.Parse(a)).ToArray();
      int[] command = Console.ReadLine().Split(' ').Select(a => int.Parse(a)).ToArray();
      int[] result = Console.ReadLine().Substring(9, line.Length - 10).Split(new[] {", "}, StringSplitOptions.None).Select(a => int.Parse(a)).ToArray();
      Console.ReadLine();

      int codes = commands.Values.Where(cmd => {
        int[] output = (int[]) registers.Clone();
        cmd.Run(command, output);

        return output.SequenceEqual(result);
      }).Count();

      if (codes >= 3) {
        triples++;
      }
    }

    Console.WriteLine(triples);

  }
}

public class Command {
  private Func<int, int, int> operation;
  private bool firstIsRegister;
  private bool secondIsRegister;

  public Command(bool firstIsRegister, bool secondIsRegister, Func<int, int, int> operation) {
    this.operation = operation;
    this.firstIsRegister = firstIsRegister;
    this.secondIsRegister = secondIsRegister;
  }

  int FirstValue(int[] command, int[] registers) {
    return this.firstIsRegister ? registers[command[1]] : command[1];
  }

  int SecondValue(int[] command, int[] registers) {
    return this.secondIsRegister ? registers[command[2]] : command[2];
  }

  public void Run(int[] command, int[] registers) {
    registers[command[3]] = this.operation(FirstValue(command, registers), SecondValue(command, registers));
  }
}

