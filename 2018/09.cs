using System;
using System.Collections.Generic;
using System.Linq;

public class Adcent09 {
  public static void Main() {
    const int ELVES = 425;
    const int MARBLE_COUNT = 7084800;
    var scores = new long[ELVES];
    var circle = new LinkedList<int>();
    circle.AddFirst(0);
    LinkedListNode<int> current = circle.First;

    int elf = 0;
    for (int i = 1; i <= MARBLE_COUNT; i++) {
      if (i % 23 == 0) {
        var toRemove = current;
        for (int j = 0; j < 7; j++) {
          toRemove = PrevInCircle(toRemove);
        }

        scores[elf] += i + toRemove.Value;
        current = NextInCircle(toRemove);
        circle.Remove(toRemove);
      } else {
        current = NextInCircle(current);
        circle.AddAfter(current, i);
        current = NextInCircle(current);
      }

      elf = (elf + 1) % ELVES;
    }

    Console.WriteLine(scores.Max());
  }

  public static LinkedListNode<T> NextInCircle<T>(LinkedListNode<T> node) {
    if (node.Next != null) {
      return node.Next;
    }

    return node.List.First;
  }

  public static LinkedListNode<T> PrevInCircle<T>(LinkedListNode<T> node) {
    if (node.Previous != null) {
      return node.Previous;
    }

    return node.List.Last;
  }
}
