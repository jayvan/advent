using System;
using System.Linq;

public class Advent09 {
  public static void Main () {
    const int LIST_SIZE = 256;
    int currentPosition = 0;
    int skipSize = 0;
    int[] list = new int[LIST_SIZE];
    for (int i = 0; i < LIST_SIZE; i++) {
      list[i] = i;
    }

    int[] lengths = Console.ReadLine().Split(',').Select(l => int.Parse(l)).ToArray();

    foreach (int length in lengths) {
      // Reverse the order of length elements in the list, starting with the element at the current position.
      for (int i = 0; i < length / 2; i++) {
        // 0 1 2 3 4 5
        //   ^   x
        int idxA = (currentPosition + i) % LIST_SIZE;
        int idxB = ((currentPosition + length - i) - 1) % LIST_SIZE;
        int tmp = list[idxA];
        list[idxA] = list[idxB];
        list[idxB] = tmp;
      }

      // Move the current position forward by that length plus the skip size.
      // Increase the skip size by 1
      currentPosition += length + skipSize++;
    }

    Console.WriteLine("Checksum: " + (list[0] * list[1]));
  }
}
