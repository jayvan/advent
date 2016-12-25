using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent252 {
  interface Instruction {
    int Run(int[] registers, out int clock);
  }

  class AddInstruction : Instruction {
    readonly int targetRegister;
    readonly int delta;

    public AddInstruction(int targetRegister, int delta) {
      this.targetRegister = targetRegister;
      this.delta = delta;
    }

    public int Run(int[] registers, out int clock) {
      registers[targetRegister] += delta;
      clock = -1;
      return 1;
    }
  }

  class CpyRegInstruction : Instruction {
    readonly int sourceRegister;
    readonly int targetRegister;

    public CpyRegInstruction(int sourceRegister, int targetRegister) {
      this.sourceRegister = sourceRegister;
      this.targetRegister = targetRegister;
    }

    public int Run(int[] registers, out int clock) {
      clock = -1;
      registers[targetRegister] = registers[sourceRegister];
      return 1;
    }
  }

  class CpyValInstruction : Instruction {
    readonly int val;
    readonly int targetRegister;

    public CpyValInstruction(int val, int targetRegister) {
      this.val = val;
      this.targetRegister = targetRegister;
    }

    public int Run(int[] registers, out int clock) {
      clock = -1;
      registers[targetRegister] = val;
      return 1;
    }
  }

  class JnzInstruction : Instruction {
    readonly int register;
    readonly int offset;

    public JnzInstruction(int register, int offset) {
      this.register = register;
      this.offset = offset;
    }

    public int Run(int[] registers, out int clock) {
      clock = -1;
      return registers[register] == 0 ? 1 : offset;
    }
  }

  class JmpInstruction : Instruction {
    readonly int offset;

    public JmpInstruction(int offset) {
      this.offset = offset;
    }

    public int Run(int[] registers, out int clock) {
      clock = -1;
      return offset;
    }
  }

  class OutInstruction : Instruction {
    readonly int register;

    public OutInstruction(int register) {
      this.register = register;
    }

    public int Run(int[] registers, out int clock) {
      clock = registers[register];
      return 1;
    }
  }

  public static void Main () {
    var instructions = new List<Instruction>();
    var regex = new Regex(@"(?:cpy ([a-d]) ([a-d])|cpy (\d+) ([a-d])|inc ([a-d])|dec ([a-d])|jnz ([a-d]) (-?\d+)|jnz (\d+) (-?\d+)|add ([a-d]) (\d+)|out ([a-d]))");

    string line;
    while ((line = Console.ReadLine()) != null) {
      Match match = regex.Match(line);
      if (match.Groups[1].Success) {
        instructions.Add(new CpyRegInstruction(match.Groups[1].Value[0] - 'a', match.Groups[2].Value[0] - 'a'));
      } else if (match.Groups[3].Success) {
        instructions.Add(new CpyValInstruction(int.Parse(match.Groups[3].Value), match.Groups[4].Value[0] - 'a'));
      } else if (match.Groups[5].Success) {
        instructions.Add(new AddInstruction(match.Groups[5].Value[0] - 'a', 1));
      } else if (match.Groups[6].Success) {
        instructions.Add(new AddInstruction(match.Groups[6].Value[0] - 'a', -1));
      } else if (match.Groups[7].Success) {
        instructions.Add(new JnzInstruction(match.Groups[7].Value[0] - 'a', int.Parse(match.Groups[8].Value)));
      } else if (match.Groups[9].Success) {
        instructions.Add(new JmpInstruction(int.Parse(match.Groups[9].Value) == 0 ? 1 : int.Parse(match.Groups[10].Value)));
      } else if (match.Groups[11].Success) {
        instructions.Add(new AddInstruction(match.Groups[11].Value[0] - 'a', int.Parse(match.Groups[12].Value)));
      } else if(match.Groups[13].Success) {
        instructions.Add(new OutInstruction(match.Groups[13].Value[0] - 'a'));
      } else {
        Console.WriteLine("Failed to parse instruction: " + line);
      }
    }

    for (int initial = 0; initial < int.MaxValue; initial++) {
      int pc = 0;
      var registers = new int[4];
      registers[0] = initial;
      int expectedClock = 0;
      int clock = -1;
      int clockMatches = 0;

      while (pc < instructions.Count) {
        pc += instructions[pc].Run(registers, out clock);
        if (clock != -1) {
          if (clock != expectedClock) {
            break;
          }
          expectedClock = expectedClock == 0 ? 1 : 0;
          clockMatches++;
          if(clockMatches == 1000) {
            Console.WriteLine("The answer is probably " + initial);
            Environment.Exit(0);
          }
        }
      }
    }
  }
}
