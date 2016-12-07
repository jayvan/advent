using System;
using System.Collections.Generic;

// I'm sure this can be done with regular expressions...

public class Advent07 {
  public static void Main () {
    int tlsCount = 0;
    int sslCount = 0;
    string line;

    while ((line = Console.ReadLine()) != null) {
      if (isTls(line)) {
        tlsCount++;
      }

      if (isSsl(line)) {
        sslCount++;
      }
    }

    Console.WriteLine("TLS IPs: " + tlsCount);
    Console.WriteLine("SSL IPs: " + sslCount);
  }

  private static bool isTls(string line) {
    bool abbaFound = false;
    bool inBrackets = false;

    for (int i = 0; i < line.Length - 3; i++) {
      if (line[i] == '[') {
        inBrackets = true;
        continue;
      }

      if (line[i] == ']') {
        inBrackets = false;
        continue;
      }

      if (line[i] != line[i + 1] && line[i + 1] == line[i + 2] && line[i] == line[i + 3]) {
        if (inBrackets) {
          return false;
        }

        abbaFound = true;
      }
    }

    return abbaFound;
  }

  private static bool isSsl(string line) {
    var abas = new List<string>();
    var babs = new List<string>();
    bool inBrackets = false;

    for (int i = 0; i < line.Length - 2; i++) {
      if (line[i] == '[') {
        inBrackets = true;
        continue;
      }

      if (line[i] == ']') {
        inBrackets = false;
        continue;
      }

      if (line[i] != line[i + 1] && line[i] == line[i + 2]) {
        if (inBrackets) {
          string sub = line.Substring(i + 1, 2);

          if (abas.Contains(sub)) {
            return true;
          }

          babs.Add(sub);
        } else {
          string sub = line.Substring(i, 2);

          if (babs.Contains(sub)) {
            return true;
          }

          abas.Add(sub);
        }
      }
    }

    return false;

  }
}
