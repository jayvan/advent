using System;
using System.Collections.Generic;

public class Advent19 {
  public static void Main () {
    int elfCount = int.Parse(Console.ReadLine());

    var elves = new LinkedList<int>();
    for (int i = 1; i <= elfCount; i++) {
      elves.AddLast(i);
    }

    LinkedListNode<int> currentNode = elves.First;
    LinkedListNode<int> oppositeNode = elves.First;

    for (int i = 0; i < elfCount / 2; i++) {
      oppositeNode = oppositeNode.Next;
    }

    while (elfCount > 1) {
      var nextNode = oppositeNode.Next;
      if (nextNode == null) {
        nextNode = elves.First;
      }

      elves.Remove(oppositeNode);

      elfCount--;
      if (elfCount % 2 == 0) {
        nextNode = nextNode.Next;
        if (nextNode == null) {
          nextNode = elves.First;
        }
      }
      oppositeNode = nextNode;

      currentNode = currentNode.Next;
      if (currentNode == null) {
        currentNode = elves.First;
      }
    }

    Console.WriteLine("Last Elf standing: " + (elves.First.Value));
  }
}
