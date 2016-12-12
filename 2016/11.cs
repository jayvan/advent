using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent11 {
  class State {
    readonly int elevatorFloor;
    readonly int[] microchipFloors;
    readonly int[] generatorFloors;

    public State(int elevatorFloor, int[] microchipFloors, int[] generatorFloors) {
      this.elevatorFloor = elevatorFloor;
      this.microchipFloors = microchipFloors;
      this.generatorFloors = generatorFloors;
    }

    public string Hash {
      get {
        string hash = elevatorFloor.ToString();

        for (int i = 0; i < microchipFloors.Length; i++) {
          hash += microchipFloors[i];
          hash += generatorFloors[i];
        }

        return hash;
      }
    }

    // A state is invalid if any microchip is on the same floor as any
    // generator other than its own, and its own generator is not present
    public bool Valid {
      get {
        for (int i = 0; i < microchipFloors.Length; i++) {
          if (microchipFloors[i] != generatorFloors[i] &&
              Array.IndexOf(generatorFloors, microchipFloors[i]) != -1) {
            return false;
          }
        }

        return true;
      }
    }

    public bool Finished {
      get {
        for (int i = 0; i < microchipFloors.Length; i++) {
          if (microchipFloors[i] != 3) {
            return false;
          }

          if (generatorFloors[i] != 3) {
            return false;
          }
        }

        return true;
      }
    }

    public IEnumerable<State> NextStates() {
      var adjacentFloors = new List<int>();
      var generatorsOnFloor = GeneratorsOnCurrentFloor;
      var chipsOnFloor = MicrochipsOnCurrentFloor;

      if (elevatorFloor < 3) {
        adjacentFloors.Add(elevatorFloor + 1);
      }

      if (elevatorFloor > 0) {
        adjacentFloors.Add(elevatorFloor - 1);
      }

      // We can take 1 generator, 1 microchip,
      // 2 generators, 2 microchips, or 1 generator & 1 microchip
      foreach (int nextFloor in adjacentFloors) {
        for (int i = 0; i < generatorsOnFloor.Count; i++) {
          int generatorA = generatorsOnFloor[i];
          var moveJustThisGenerator = (int[])generatorFloors.Clone();
          moveJustThisGenerator[generatorA] = nextFloor;
          // Move just 1 generator
          yield return new State(nextFloor, microchipFloors, moveJustThisGenerator);

          for (int j = i + 1; j < generatorsOnFloor.Count; j++) {
            int generatorB = generatorsOnFloor[j];
            var moveTwoGenerators = (int[])moveJustThisGenerator.Clone();
            moveTwoGenerators[generatorB] = nextFloor;
            // Move 2 generators
            yield return new State(nextFloor, microchipFloors, moveTwoGenerators);
          }

          foreach (int chip in chipsOnFloor) {
            var moveChipWithGenerator = (int[])microchipFloors.Clone();
            moveChipWithGenerator[chip] = nextFloor;
            // Move 1 generator and 1 chip
            yield return new State(nextFloor, moveChipWithGenerator, moveJustThisGenerator);
          }
        }

        for (int i = 0; i < chipsOnFloor.Count; i++) {
          int chipA = chipsOnFloor[i];
          var moveJustThisChip = (int[])microchipFloors.Clone();
          moveJustThisChip[chipA] = nextFloor;
          // Move just 1 chip
          yield return new State(nextFloor, moveJustThisChip, generatorFloors);

          for (int j = i + 1; j < chipsOnFloor.Count; j++) {
            int chipB = chipsOnFloor[j];
            var moveTwoChips = (int[])moveJustThisChip.Clone();
            moveTwoChips[chipB] = nextFloor;
            // Move 2 chips
            yield return new State(nextFloor, moveTwoChips, generatorFloors);
          }
        }
      }
    }

    private List<int> MicrochipsOnCurrentFloor {
      get {
        var chipsOnFloor = new List<int>();

        for (int i = 0; i < microchipFloors.Length; i++) {
          if (microchipFloors[i] == elevatorFloor) {
            chipsOnFloor.Add(i);
          }
        }

        return chipsOnFloor;
      }
    }

    private List<int> GeneratorsOnCurrentFloor {
      get {
        var generatorsOnFloor = new List<int>();

        for (int i = 0; i < generatorFloors.Length; i++) {
          if (generatorFloors[i] == elevatorFloor) {
            generatorsOnFloor.Add(i);
          }
        }

        return generatorsOnFloor;
      }
    }

    public override string ToString() {
      String s = "";
      for (int i = 3; i >= 0; i--) {
        s += i + 1 + ": ";

        if (elevatorFloor == i) {
          s += "E ";
        }

        for (int j = 0; j < microchipFloors.Length; j++) {
          if (microchipFloors[j] == i) {
            s += j + "M ";
          }
        }

        for (int j = 0; j < generatorFloors.Length; j++) {
          if (generatorFloors[j] == i) {
            s += j + "G ";
          }
        }

        s += "\n";
      }

      return s;
    }
  }

  public static void Main () {
    State initialState = ReadInput();

    int steps = 1;
    var currentStates = new List<State>();
    var nextStates = new List<State>();
    var visited = new HashSet<string>();
    currentStates.Add(initialState);

    while (true) {
      foreach (State state in currentStates) {
        foreach (State nextState in state.NextStates()) {
          if (nextState.Finished) {
            Console.WriteLine("Found a valid solution in " + steps + " steps!");
            return;
          }

          if (nextState.Valid) {
            string hash = nextState.Hash;

            if (!visited.Contains(hash)) {
              nextStates.Add(nextState);
              visited.Add(hash);
            }
          }
        }
      }

      currentStates = nextStates;
      nextStates = new List<State>();
      steps++;
    }
  }

  private static State ReadInput() {
    var contains = new Regex("(?:a ([a-z]+)-compatible|([a-z]+) generator)");
    var microchipFloors = new Dictionary<string, int>();
    var generatorFloors = new Dictionary<string, int>();

    for (int i = 0; i < 4; i++) {
      string line = Console.ReadLine();

      foreach (Match match in contains.Matches(line)) {
        if (match.Groups[1].Success) {
          microchipFloors.Add(match.Groups[1].ToString(), i);
        } else {
          generatorFloors.Add(match.Groups[2].ToString(), i);
        }
      }
    }

    var microchipLocations = new int[microchipFloors.Count];
    var generatorLocations = new int[generatorFloors.Count];

    int j = 0;
    foreach (string element in microchipFloors.Keys) {
      microchipLocations[j] = microchipFloors[element];
      generatorLocations[j] = generatorFloors[element];
      j++;
    }

    return new State(0, microchipLocations, generatorLocations);
  }
}
