using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent21 {
  public static void Main () {
    var swapPosition = new Regex("swap position ([0-9]+) with position ([0-9]+)");
    var swapLetter = new Regex("swap letter ([a-z]) with letter ([a-z])");
    var rotate = new Regex("rotate (left|right) ([0-9]+) step");
    var rotateByPosition = new Regex("rotate based on position of letter ([a-z])");
    var reverse = new Regex("reverse positions ([0-9]+) through ([0-9]+)");
    var move = new Regex("move position ([0-9]+) to position ([0-9]+)");

    var code = new char[] {'f', 'b', 'g', 'd', 'c', 'e', 'a', 'h'};
    var rotationAroundLookup = new Dictionary<int, int>();

    for (int i = 0; i < code.Length; i++) {
      var trainingCode = (char[])code.Clone();
      char initialChar = code[i];
      int rotationAmount = i + 1;
      if (i >= 4) {
        rotationAmount++;
      }

      Rotate(trainingCode, rotationAmount);
      int finalIndex = Array.IndexOf(trainingCode, initialChar);

      rotationAroundLookup.Add(finalIndex, -rotationAmount);
    }

    var commands = new List<string>();
    string line;

    while ((line = Console.ReadLine()) != null) {
      commands.Add(line);
    }

    commands.Reverse();

    foreach (string command in commands) {
      Match match;

      if ((match = swapPosition.Match(command)).Success) {
        int positionA = int.Parse(match.Groups[1].Value);
        int positionB = int.Parse(match.Groups[2].Value);
        char tmp = code[positionA];
        code[positionA] = code[positionB];
        code[positionB] = tmp;
      } else if ((match = swapLetter.Match(command)).Success) {
        char letterA = match.Groups[1].Value[0];
        char letterB = match.Groups[2].Value[0];

        for (int i = 0; i < code.Length; i++) {
          if (code[i] == letterA) {
            code[i] = letterB;
          } else if (code[i] == letterB) {
            code[i] = letterA;
          }
        }
      } else if ((match = rotate.Match(command)).Success) {
        int rotationAmount = int.Parse(match.Groups[2].Value);
        if (match.Groups[1].Value == "right") {
          rotationAmount *= -1;
        }

        Rotate(code, rotationAmount);
      } else if ((match = rotateByPosition.Match(command)).Success) {
        int indexOfLetter = Array.IndexOf(code, match.Groups[1].Value[0]);
        int rotationAmount = rotationAroundLookup[indexOfLetter];
        Rotate(code, rotationAmount);
      } else if ((match = reverse.Match(command)).Success) {
        int startIndex = int.Parse(match.Groups[1].Value);
        int endIndex = int.Parse(match.Groups[2].Value);
        int length = (endIndex - startIndex + 1) / 2;

        for (int i = 0; i < length; i++) {
          Swap(code, startIndex + i, endIndex - i);
        }
      } else if ((match = move.Match(command)).Success) {
        string tmp = new String(code);
        int endPosition = int.Parse(match.Groups[1].Value);
        int initialPosition = int.Parse(match.Groups[2].Value);
        char c = tmp[initialPosition];
        tmp = tmp.Remove(initialPosition, 1);
        tmp = tmp.Insert(endPosition, c.ToString());
        code = tmp.ToCharArray();
      }

      Console.WriteLine(code);
    }
  }

  private static void Swap(char[] text, int positionA, int positionB) {
    char tmp = text[positionA];
    text[positionA] = text[positionB];
    text[positionB] = tmp;
  }

  private static void Rotate(char[] text, int amount) {
    char[] backup = new char[text.Length];
    Array.Copy(text, backup, text.Length);

    for (int i = 0; i < text.Length; i++) {
      int takeIndex = (i - amount) % text.Length;

      if (takeIndex < 0) {
        takeIndex += text.Length;
      }

      text[i] = backup[takeIndex];
    }
  }
}
