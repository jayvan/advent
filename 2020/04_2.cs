using System;
using System.Text.RegularExpressions;

public class Advent042 {
  public static Regex[] regexes = {
    new Regex("byr:(19[2-9][0-9]|200[0-2])\\s"),
    new Regex("iyr:(201[0-9]|2020)\\s"),
    new Regex("eyr:(202[0-9]|2030)\\s"),
    new Regex("hgt:(((1[5-8][0-9])|19[0-3])cm|(59|6[0-9]|7[0-6])in)\\s"),
    new Regex("hcl:#[0-9a-f]{6}\\s"),
    new Regex("ecl:(amb|blu|brn|gry|grn|hzl|oth)\\s"),
    new Regex("pid:[0-9]{9}\\s"),
  };

  public static void Main() {
    string line;
    int valid = 0;
    string passport = string.Empty;

    while (true) {
      line = Console.ReadLine();

      if (string.IsNullOrEmpty(line)) {
        if (IsValid(passport)) {
          valid++;
        }

        passport = string.Empty;

        if (line == null) {
          break;
        }
      } else {
        passport += line + ' ';
      }
    }

    Console.WriteLine(valid);
  }

  private static bool IsValid(string passport) {
    foreach (Regex regex in regexes) {
      Match match = regex.Match(passport);

      if (!match.Success) {
        return false;
      }
    }

    return true;
  }
}
