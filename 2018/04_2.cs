using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace advent2018 {
  public class Advent04 {
    public static void Main() {
      string line;
      int maxSleepInMinute = 0;
      int answer = -1;
      int currentGuard = 0;
      int sleepTime = 0;
      var sleepPerGuardAndMinute = new Dictionary<Tuple<int, int>, int>();
      var shiftRegex = new Regex(@"Guard #(\d+) begins shift");
      var sleepRegex = new Regex(@"(\d+)] falls asleep");
      var wakeRegex = new Regex(@"(\d+)] wakes up");

      while ((line = Console.ReadLine()) != null) {
        Match match = shiftRegex.Match(line);

        if (match.Success) {
          currentGuard = int.Parse(match.Groups[1].Value);
          continue;
        }

        match = sleepRegex.Match(line);
        if (match.Success) {
          sleepTime = int.Parse(match.Groups[1].Value);
        }

        match = wakeRegex.Match(line);
        if (match.Success) {
          int endTime = int.Parse(match.Groups[1].Value);

          for (int i = sleepTime; i < endTime; i++) {
            var guardMinute = new Tuple<int, int>(currentGuard, i);
            if (!sleepPerGuardAndMinute.ContainsKey(guardMinute)) {
              sleepPerGuardAndMinute.Add(guardMinute, 0);
            }

            sleepPerGuardAndMinute[guardMinute]++;

            if (sleepPerGuardAndMinute[guardMinute] > maxSleepInMinute) {
              maxSleepInMinute = sleepPerGuardAndMinute[guardMinute];
              answer = currentGuard * i;
            }
          }
        }
      }

      Console.WriteLine(answer);
    }
  }
}
