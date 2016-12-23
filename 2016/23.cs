using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent23 {
  public static void Main () {
    var instructions = new List<string>();
    var regex = new Regex(@"(?:cpy ([a-d]) ([a-d])|cpy (-?\d+) ([a-d])|inc ([a-d])|dec ([a-d])|jnz ([a-d]) (-?\d+)|jnz (\d+) (\d+)|jnz (\d+) ([a-d])|tgl ([a-z]))");

    string line;
    while ((line = Console.ReadLine()) != null) {
      instructions.Add(line);
    }

    int pc = 0;
    var registers = new int[4];
    registers[0] = 7;

    while (pc < instructions.Count) {
      Match match = regex.Match(instructions[pc]);

      if (match.Groups[1].Success) {
        int srcReg = match.Groups[1].Value[0] - 'a';
        int dstReg = match.Groups[2].Value[0] - 'a';
        registers[dstReg] = registers[srcReg];
        pc++;
      } else if (match.Groups[3].Success) {
        int srcVal = int.Parse(match.Groups[3].Value);
        int dstReg = match.Groups[4].Value[0] - 'a';
        registers[dstReg] = srcVal;
        pc++;
      } else if (match.Groups[5].Success) {
        int reg = match.Groups[5].Value[0] - 'a';
        registers[reg]++;
        pc++;
      } else if (match.Groups[6].Success) {
        int reg = match.Groups[6].Value[0] - 'a';
        registers[reg]--;
        pc++;
      } else if (match.Groups[7].Success) {
        int reg = match.Groups[7].Value[0] - 'a';
        int offset = int.Parse(match.Groups[8].Value);
        if (registers[reg] != 0) {
          pc += offset;
        } else {
          pc++;
        }
      } else if (match.Groups[9].Success) {
        int val = int.Parse(match.Groups[9].Value);
        int offset = int.Parse(match.Groups[10].Value);
        if (val != 0) {
          pc += offset;
        } else {
          pc++;
        }
      } else if (match.Groups[11].Success) {
        int val = int.Parse(match.Groups[11].Value);
        int reg = match.Groups[12].Value[0] - 'a';
        if (val != 0) {
          pc += registers[reg];
        } else {
          pc++;
        }
      } else if (match.Groups[13].Success) {
        int index = pc + registers[match.Groups[13].Value[0] - 'a'];

        if (index > 0 && index < instructions.Count) {
          string instruction = instructions[index];
          if (instruction.Split(new char[] {' '}).Length == 2) {
            // one-argument
            if (instruction.Contains("inc")) {
              instructions[index] = instruction.Replace("inc", "dec");
            } else {
              instructions[index] = "inc" + instruction.Substring(3);
            }
          } else {
            // two-arguments
            if (instruction.Contains("jnz")) {
              instructions[index] = instruction.Replace("jnz", "cpy");
            } else {
              instructions[index] = "jnz" + instruction.Substring(3);
            }
          }
        }

        pc++;
      } else {
        Console.WriteLine("Failed to parse instruction: " + line);
        pc++;
      }
      Console.WriteLine($"pc: {pc}, a: {registers[0]}, b: {registers[1]}, c: {registers[2]}, d: {registers[3]}" );
    }

    Console.WriteLine($"pc: {pc}, a: {registers[0]}, b: {registers[1]}, c: {registers[2]}, d: {registers[3]}" );
  }
}
