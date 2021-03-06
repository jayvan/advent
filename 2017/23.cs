﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent18 {
  public struct RunResult {
    public readonly long PcOffset;
    public readonly bool Multiplication;

    public RunResult (long offset, bool multiplication = false) {
      PcOffset = offset;
      Multiplication = multiplication;
    }
  }

  public interface IValue {
    long Value { get; }
  }

  public class Register : IValue {
    private long val;

    public long Value {
      get {
        return this.val;
      }

      set {
        this.val = value;
      }
    }
  }

  public class Const : IValue {
    private long val;

    public Const(long val) {
      this.val = val;
    }

    public long Value {
      get {
        return this.val;
      }
    }
  }

  public class Simulation {
    private static readonly Regex regex = new Regex(@"(?:set ([a-z]) ([a-z])|set ([a-z]) (-?\d+)|add ([a-z]) ([a-z])|add ([a-z]) (-?\d+)|mul ([a-z]) ([a-z])|mul ([a-z]) (-?\d+)|mod ([a-z]) ([a-z])|mod ([a-z]) (\d+)|jnz ([a-z]) ([a-z])|jnz ([a-z]) (-?\d+)|jnz (-?\d+) (-?\d+)|rcv ([a-z])|snd ([a-z])|sub ([a-z]) ([a-z])|sub ([a-z]) (-?\d+))");
    public int multiplications { get; private set; }
    private Dictionary<char, Register> registers = new Dictionary<char, Register>();
    private List<Instruction> program = new List<Instruction>();
    private long pc;

    public Simulation() {
      var reg = new Register();
      reg.Value = 1;
      this.registers.Add('a', reg);
    }

    public void AddInstruction(string instruction) {
      Match match = regex.Match(instruction);
      if (match.Groups[1].Success) {
        program.Add(new SetInstruction(this.GetRegister(match.Groups[1].Value[0]), this.GetRegister(match.Groups[2].Value[0])));
      } else if (match.Groups[3].Success) {
        program.Add(new SetInstruction(this.GetRegister(match.Groups[3].Value[0]), new Const(int.Parse(match.Groups[4].Value))));
      } else if (match.Groups[5].Success) {
        program.Add(new AddInstruction(this.GetRegister(match.Groups[5].Value[0]), this.GetRegister(match.Groups[6].Value[0])));
      } else if (match.Groups[7].Success) {
        program.Add(new AddInstruction(this.GetRegister(match.Groups[7].Value[0]), new Const(int.Parse(match.Groups[8].Value))));
      } else if (match.Groups[9].Success) {
        program.Add(new MulInstruction(this.GetRegister(match.Groups[9].Value[0]), this.GetRegister(match.Groups[10].Value[0])));
      } else if (match.Groups[11].Success) {
        program.Add(new MulInstruction(this.GetRegister(match.Groups[11].Value[0]), new Const(int.Parse(match.Groups[12].Value))));
      } else if (match.Groups[13].Success) {
        program.Add(new ModInstruction(this.GetRegister(match.Groups[13].Value[0]), this.GetRegister(match.Groups[14].Value[0])));
      } else if (match.Groups[15].Success) {
        program.Add(new ModInstruction(this.GetRegister(match.Groups[15].Value[0]), new Const(int.Parse(match.Groups[16].Value))));
      } else if (match.Groups[17].Success) {
        program.Add(new JnzInstruction(this.GetRegister(match.Groups[17].Value[0]), this.GetRegister(match.Groups[18].Value[0])));
      } else if (match.Groups[19].Success) {
        program.Add(new JnzInstruction(this.GetRegister(match.Groups[19].Value[0]), new Const(int.Parse(match.Groups[20].Value))));
      } else if (match.Groups[21].Success) {
        program.Add(new JnzInstruction(new Const(int.Parse(match.Groups[21].Value)), new Const(int.Parse(match.Groups[22].Value))));
      } else if (match.Groups[23].Success) {
        program.Add(new RcvInstruction(this.GetRegister(match.Groups[23].Value[0])));
      } else if (match.Groups[24].Success) {
        program.Add(new SndInstruction(this.GetRegister(match.Groups[24].Value[0])));
      } else if (match.Groups[25].Success) {
        program.Add(new SubInstruction(this.GetRegister(match.Groups[25].Value[0]), this.GetRegister(match.Groups[26].Value[0])));
      } else if (match.Groups[27].Success) {
        program.Add(new SubInstruction(this.GetRegister(match.Groups[27].Value[0]), new Const(int.Parse(match.Groups[28].Value))));
      } else {
        Console.WriteLine("Failed to parse instruction: " + instruction);
      }
    }

    public long Run() {
      while (pc >= 0 && pc < program.Count) {
        RunResult result = program[(int)pc].Run();
        pc += result.PcOffset;

        if (result.Multiplication) {
          this.multiplications++;
        }
      }

      return GetRegister('h').Value;
    }

    private Register GetRegister(char name) {
      if (registers.ContainsKey(name)) {
        return registers[name];
      }

      var reg = new Register();
      if (name == 'a') {
        reg.Value = 1;
      }
      registers.Add(name, reg);
      return reg;
    }
  }

  interface Instruction {
    RunResult Run();
  }

  class SetInstruction : Instruction {
    readonly Register destination;
    readonly IValue delta;

    public SetInstruction(Register destination, IValue delta) {
      this.destination = destination;
      this.delta = delta;
    }

    public RunResult Run() {
      destination.Value = delta.Value;
      return new RunResult(1);
    }
  }
  class AddInstruction : Instruction {
    readonly Register destination;
    readonly IValue delta;

    public AddInstruction(Register destination, IValue delta) {
      this.destination = destination;
      this.delta = delta;
    }

    public RunResult Run() {
      destination.Value += delta.Value;
      return new RunResult(1);
    }
  }

  class SubInstruction : Instruction {
    readonly Register destination;
    readonly IValue delta;

    public SubInstruction(Register destination, IValue delta) {
      this.destination = destination;
      this.delta = delta;
    }

    public RunResult Run() {
      destination.Value -= delta.Value;
      return new RunResult(1);
    }
  }

  class MulInstruction : Instruction {
    readonly Register destination;
    readonly IValue product;

    public MulInstruction(Register destination, IValue product) {
      this.destination = destination;
      this.product = product;
    }

    public RunResult Run() {
      destination.Value *= product.Value;
      return new RunResult(1, true);
    }
  }

  class ModInstruction : Instruction {
    readonly Register destination;
    readonly IValue divisor;

    public ModInstruction(Register destination, IValue divisor) {
      this.destination = destination;
      this.divisor = divisor;
    }

    public RunResult Run() {
      destination.Value %= divisor.Value;
      return new RunResult(1);
    }
  }

  class JnzInstruction : Instruction {
    readonly IValue check;
    readonly IValue offset;

    public JnzInstruction(IValue check, IValue offset) {
      this.check = check;
      this.offset = offset;
    }

    public RunResult Run() {
      if (this.check.Value != 0) {
        return new RunResult(this.offset.Value);
      }

      return new RunResult(1);
    }
  }

  class RcvInstruction : Instruction {
    readonly IValue check;

    public RcvInstruction(IValue check) {
      this.check = check;
    }

    public RunResult Run() {
      if (this.check.Value != 0) {
        return new RunResult(1);
      }

      return new RunResult(1);
    }
  }

  class SndInstruction : Instruction {
    readonly IValue check;

    public SndInstruction(IValue check) {
      this.check = check;
    }

    public RunResult Run() {
      if (this.check.Value != 0) {
        return new RunResult(1);
      }

      return new RunResult(1);
    }
  }

  public static void Main () {
    var simulation = new Simulation();

    string line;
    while ((line = Console.ReadLine()) != null) {
      simulation.AddInstruction(line);
    }

    Console.WriteLine(simulation.Run());
  }
}
