using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace advent2018 {
  public class Advent04 {
    public static void Main() {
      string line;
      int longestSleep = 0;
      int longestSleepingGuard = 0;
      int currentGuard = 0;
      int sleepTime = 0;
      var sleepPerGuard = new Dictionary<int, int>();
      var sleepPerGuardAndMinute = new Dictionary<Tuple<int, int>, int>();
      var shiftRegex = new Regex(@"Guard #(\d+) begins shift");
      var sleepRegex = new Regex(@"(\d+)] falls asleep");
      var wakeRegex = new Regex(@"(\d+)] wakes up");

      while ((line = Console.ReadLine()) != null) {
        Match match = shiftRegex.Match(line);

        if (match.Success) {
          currentGuard = int.Parse(match.Groups[1].Value);
          if (!sleepPerGuard.ContainsKey(currentGuard)) {
            sleepPerGuard.Add(currentGuard, 0);
          }
          continue;
        }

        match = sleepRegex.Match(line);
        if (match.Success) {
          sleepTime = int.Parse(match.Groups[1].Value);
        }

        match = wakeRegex.Match(line);
        if (match.Success) {
          int endTime = int.Parse(match.Groups[1].Value);
          int sleep = endTime - sleepTime;
          sleepPerGuard[currentGuard] += sleep;
          if (sleepPerGuard[currentGuard] > longestSleep) {
            longestSleep = sleepPerGuard[currentGuard];
            longestSleepingGuard = currentGuard;
          }

          for (int i = sleepTime; i < endTime; i++) {
            var guardMinute = new Tuple<int, int>(currentGuard, i);
            if (sleepPerGuardAndMinute.ContainsKey(guardMinute)) {
              sleepPerGuardAndMinute[guardMinute] += sleep;
            } else {
              sleepPerGuardAndMinute.Add(guardMinute, sleep);
            }
          }
        }
      }

      Console.WriteLine("Longest sleeping guard: " + longestSleepingGuard);
      int maxSleepInMinute = 0;
      int maxMinute = 0;

      foreach (KeyValuePair<Tuple<int, int>, int> keyValuePair in sleepPerGuardAndMinute) {
        if (keyValuePair.Key.Item1 == longestSleepingGuard && keyValuePair.Value > maxSleepInMinute) {
          maxSleepInMinute = keyValuePair.Value;
          maxMinute = keyValuePair.Key.Item2;
        }
      }

      Console.WriteLine("Minute with most sleep: " + maxMinute);
      Console.WriteLine(longestSleepingGuard * maxMinute);
    }
  }
}
