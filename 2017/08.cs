using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Advent08 {
  public class Program {
    public int HighestValueHeld { get; private set; }
    private Dictionary<string, int> registers = new Dictionary<string, int>();

    public int HighestRegisterValue {
      get {
        return registers.Values.Max();
      }
    }

    public void RunCommand(string command) {
      string[] split = command.Split(' ');

      string targetRegister = split[0];
      string direction = split[1];
      int deltaAmount = int.Parse(split[2]);
      string checkRegister = split[4];
      string checkOperator = split[5];
      int checkValue = int.Parse(split[6]);

      if (CheckCondition(checkRegister, checkOperator, checkValue)) {
        UpdateRegister(targetRegister, direction, deltaAmount);
      }
    }

    private void UpdateRegister(string reg, string op, int val) {
      if (op == "inc") {
        SetRegister(reg, GetRegister(reg) + val);
      } else if (op == "dec") {
        SetRegister(reg, GetRegister(reg) - val);
      } else {
        throw new NotImplementedException($"Operator '{op}' is not supported");
      }

      HighestValueHeld = Math.Max(HighestValueHeld, GetRegister(reg));
    }

    private bool CheckCondition(string reg, string op, int val) {
      int regValue = GetRegister(reg);
      switch (op) {
        case ">":
          return regValue > val;
        case ">=":
          return regValue >= val;
        case "<":
          return regValue < val;
        case "<=":
          return regValue <= val;
        case "==":
          return regValue == val;
        case "!=":
          return regValue != val;
      }

      throw new NotImplementedException($"Operator '{op}' is not supported");
    }

    private void SetRegister(string register, int val) {
      if (registers.ContainsKey(register)) {
        registers.Remove(register);
      }

      registers.Add(register, val);
    }

    private int GetRegister(string register) {
      if (registers.ContainsKey(register)) {
        return registers[register];
      }

      return 0;
    }
  }

  public static void Main () {
    var program = new Program();
    string line;

    while ((line = Console.ReadLine()) != null) {
      program.RunCommand(line);
    }

    Console.WriteLine($"Highest register value is: {program.HighestRegisterValue}");
    Console.WriteLine($"Highest interim value is: {program.HighestValueHeld}");
  }
}
