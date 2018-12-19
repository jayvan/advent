using System;
using System.Collections.Generic;
using System.Linq;

public class Advent16_2 {
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

    while ((line = Console.ReadLine()) != string.Empty) {
      int[] registers = line.Substring(9, line.Length - 10).Split(new[] {", "}, StringSplitOptions.None).Select(a => int.Parse(a)).ToArray();
      int[] command = Console.ReadLine().Split(' ').Select(a => int.Parse(a)).ToArray();
      int[] result = Console.ReadLine().Substring(9, line.Length - 10).Split(new[] {", "}, StringSplitOptions.None).Select(a => int.Parse(a)).ToArray();
      Console.ReadLine();

      foreach (var kvp in commands) {
        int[] output = (int[]) registers.Clone();
        kvp.Value.Run(command, output);

        if (!output.SequenceEqual(result)) {
          kvp.Value.RemoveOpCode(command[0]);
        }
      }
    }

    while (true) {
      if (commands.Values.All(cmd => cmd.Determined)) {
        break;
      }

      foreach (Command a in commands.Values) {
        if (a.Determined) {
          foreach (Command b in commands.Values) {
            if (a != b) {
              b.RemoveOpCode(a.OpCode);
            }
          }
        }
      }
    }

    var opCodes = new Command[16];
    foreach (var cmd in commands.Values) {
      opCodes[cmd.OpCode] = cmd;
    }

    Console.ReadLine();

    int[] reg = new int[4];

    while ((line = Console.ReadLine()) != null) {
      int[] cmd = line.Split(' ').Select(a => int.Parse(a)).ToArray();
      opCodes[cmd[0]].Run(cmd, reg);
    }

    Console.WriteLine(reg[0]);
  }
}

public class Command {
  private Func<int, int, int> operation;
  private bool firstIsRegister;
  private bool secondIsRegister;
  public List<int> potentialOpCodes;
  public bool Determined => this.potentialOpCodes.Count == 1;
  public int OpCode => this.potentialOpCodes[0];

  public Command(bool firstIsRegister, bool secondIsRegister, Func<int, int, int> operation) {
    this.operation = operation;
    this.firstIsRegister = firstIsRegister;
    this.secondIsRegister = secondIsRegister;
    this.potentialOpCodes = new List<int>();
    for (int i = 0; i < 16; i++) {
      this.potentialOpCodes.Add(i);
    }
  }

  public void RemoveOpCode(int code) {
    this.potentialOpCodes.Remove(code);
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
