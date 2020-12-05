using System;
using System.Collections.Generic;

public class Advent05 {
  public static void Main() {
    string line;
    int maxId = 0;
    int minId = int.MaxValue;
    HashSet<int> takenSeats = new HashSet<int>();

    while ((line = Console.ReadLine()) != null) {
      int seatId = SeatId(line);
      maxId = Math.Max(maxId, seatId);
      minId = Math.Min(minId, seatId);
      takenSeats.Add(seatId);
    }

    Console.WriteLine($"Seats: {minId} - {maxId}");

    for (int i = minId + 1; i < maxId; i++) {
      if (!takenSeats.Contains(i)) {
        Console.WriteLine(i);
      }
    }
  }

  private static int SeatId(string seq) {
    int maxRow = 127;
    int minRow = 0;

    for (int i = 0; i < 7; i++) {
      if (seq[i] == 'F') {
        maxRow = (minRow + maxRow) / 2;
      } else {
        minRow = (int)Math.Ceiling(((float)minRow + maxRow) / 2);
      }
    }

    int minCol = 0;
    int maxCol = 7;
    for (int i = 7; i < 10; i++) {
      if (seq[i] == 'L') {
        maxCol = (minCol + maxCol) / 2;
      } else {
        minCol = (int)Math.Ceiling(((float)minCol + maxCol) / 2);
      }
    }

    return minRow * 8 + minCol;
  }
}
