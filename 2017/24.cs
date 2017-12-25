using System;
using System.Collections.Generic;

public class Advent24 {
  public struct Component {
    public readonly int ID;
    public readonly int A;
    public readonly int B;

    public Component(int id, int a, int b) {
      ID = id;
      A = a;
      B = b;
    }

    public int Strength {
      get {
        return A + B;
      }
    }

    public bool Matches(int end) {
      return end == A || end == B;
    }

    public int Other(int end) {
      return end == A ? B : A;
    }
  }

  public struct State {
    private readonly List<int> used;
    public readonly int Ending;
    public readonly int Strength;

    public State(int ending, List<int> used, int strength = 0) {
      this.used = used;
      Ending = ending;
      Strength = strength;
    }

    public State AddComponent(Component c) {
      var newUsed = new List<int>(used);
      newUsed.Add(c.ID);
      newUsed.Sort();
      int newEnding = c.Other(Ending);
      int newStrength = Strength + c.Strength;
      return new State(newEnding, newUsed, newStrength);
    }

    public bool Used(int id) {
      return used.Contains(id);
    }

    public string Hash() {
      return string.Join("|", used) + ':' + Ending;
    }
  }

  public static void Main() {
    var components = new List<Component>();
    string line;

    while ((line = Console.ReadLine()) != null) {
      string[] parts = line.Split('/');
      components.Add(new Component(components.Count, int.Parse(parts[0]), int.Parse(parts[1])));
    }

    int strongest = 0;
    var visisted = new HashSet<string>();
    var current = new List<State>();
    var next = new List<State>();
    current.Add(new State(0, new List<int>()));

    while (current.Count != 0) {
      foreach (State state in current) {
        strongest = Math.Max(strongest, state.Strength);

        foreach (Component component in components) {
          if (component.Matches(state.Ending) && !state.Used(component.ID)) {
            State nextState = state.AddComponent(component);
            string hash = nextState.Hash();
            if (!visisted.Contains(hash)) {
              visisted.Add(hash);
              next.Add(nextState);
            }
          }
        }
      }

      current = next;
      next = new List<State>();
    }

    Console.WriteLine(strongest);
  }
}
