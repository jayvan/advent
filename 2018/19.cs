using System;
using System.Collections.Generic;

public class Advent19 {
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

    List<Instruction> program = new List<Instruction>();
    string line = Console.ReadLine();
    int pci = line[line.Length - 1] - '0';
    int pc = 0;
    int[] registers = new int[6];

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split(' ');
      Command command = commands[parts[0]];
      int[] args = new int[3];
      for (int i = 0; i < 3; i++) {
        args[i] = int.Parse(parts[i + 1]);
      }
      program.Add(new Instruction(command, args));
    }

    while (pc >= 0 && pc < program.Count) {
      registers[pci] = pc;
      program[pc].Run(registers);
      pc = registers[pci] + 1;
    }

    Console.WriteLine(registers[0]);
  }

  public class Instruction {
    private Command command;
    private int[] arguments;

    public Instruction(Command command, int[] arguments) {
      this.command = command;
      this.arguments = arguments;
    }

    public void Run(int[] registers) {
      this.command.Run(this.arguments, registers);
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
      return this.firstIsRegister ? registers[command[0]] : command[0];
    }

    int SecondValue(int[] command, int[] registers) {
      return this.secondIsRegister ? registers[command[1]] : command[1];
    }

    public void Run(int[] command, int[] registers) {
      registers[command[2]] = this.operation(FirstValue(command, registers), SecondValue(command, registers));
    }
  }
}
