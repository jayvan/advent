using System;

public class Advent04 {
  static string[] mandatoryFields = {"byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:"};

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
    foreach (string mandatoryField in mandatoryFields) {
      if (passport.IndexOf(mandatoryField) < 0) {
        Console.WriteLine("INVALID - MISSING " + mandatoryField + ": " + passport);
        return false;
      }
    }

    Console.WriteLine("VALID: " + passport);
    return true;
  }
}
