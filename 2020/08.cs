using System;
using System.Collections.Generic;

public class Advent08 {
  struct Command {
    public enum Instruction {
      jmp,
      acc,
      nop
    }

    public Instruction instruction;
    public int offset;

    public Command(Instruction instruction, int offset) {
      this.instruction = instruction;
      this.offset = offset;
    }

    public Command Invert() {
      if (this.instruction == Instruction.acc) {
        return this;
      }

      if (this.instruction == Instruction.jmp) {
        return new Command(Instruction.nop, this.offset);
      } else {
        return new Command(Instruction.jmp, this.offset);
      }
    }
  }
  public static void Main() {
    List<Command> program = new List<Command>();
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split(' ');
      program.Add(new Command((Command.Instruction)Enum.Parse(typeof(Command.Instruction), parts[0]), int.Parse(parts[1])));
    }

    int finalAcc;
    RunProgram(program, out finalAcc);
    Console.WriteLine(finalAcc);

    for (int i = 0; i < program.Count; i++) {
      if (program[i].instruction != Command.Instruction.acc) {
        program[i] = program[i].Invert();

        bool finished = RunProgram(program, out finalAcc);
        if (finished) {
          Console.WriteLine(finalAcc);
          break;
        }

        program[i] = program[i].Invert();
      }
    }
  }

  private static bool RunProgram(List<Command> program, out int acc) {
    acc = 0;
    int pc = 0;
    bool[] visited = new bool[program.Count];

    while (pc < visited.Length) {
      if (visited[pc]) {
        return false;
      }

      visited[pc] = true;

      switch (program[pc].instruction) {
        case Command.Instruction.jmp:
          pc += program[pc].offset;
          break;
        case Command.Instruction.acc:
          acc += program[pc].offset;
          pc++;
          break;
        case Command.Instruction.nop:
          pc++;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    return true;
  }
}
