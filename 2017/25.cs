using System;
using System.Collections.Generic;

public class Advent25 {
  public class State {
    public readonly char Name;
    public readonly Action FalseAction;
    public readonly Action TrueAction;

    public State(char name, Action falseAction, Action trueAction) {
      Name = name;
      FalseAction = falseAction;
      TrueAction = trueAction;
    }

    public Action GetAction(bool state) {
      return state ? TrueAction : FalseAction;
    }
  }

  public class Action {
    public bool Val;
    public bool Right;
    public char NextState;

    public Action(bool val, bool right, char next) {
      Val = val;
      Right = right;
      NextState = next;
    }
  }

  public static void Main() {
    var states = new Dictionary<char, State>();
    string line;
    line = Console.ReadLine();
    char currentState = line[line.Length - 2];

    line = Console.ReadLine();
    int steps = int.Parse(line.Split(' ')[5]);

    // read states
    while ((line = Console.ReadLine()) != null) {
      line = Console.ReadLine();
      char stateName = line[line.Length - 2];

      //Throwaway
      Console.ReadLine();

      line = Console.ReadLine();
      bool falseValue = line[line.Length - 2] == '1';
      line = Console.ReadLine();
      bool falseMovingRight = line[line.Length - 3] == 'h';
      line = Console.ReadLine();
      char falseNextState = line[line.Length - 2];

      var falseAction = new Action(falseValue, falseMovingRight, falseNextState);

      //Throwaway
      Console.ReadLine();

      line = Console.ReadLine();
      bool trueValue = line[line.Length - 2] == '1';
      line = Console.ReadLine();
      bool trueMovingRight = line[line.Length - 3] == 'h';
      line = Console.ReadLine();
      char trueNextState = line[line.Length - 2];

      var trueAction = new Action(trueValue, trueMovingRight, trueNextState);

      states.Add(stateName, new State(stateName, falseAction, trueAction));
    }

    var tape = new Dictionary<int, bool>();
    int position = 0;
    State state = states[currentState];
    int ones = 0;

    for (int i = 0; i < steps; i++) {
      var action = state.GetAction(tape.ContainsKey(position) && tape[position]);

      if (tape.ContainsKey(position) && tape[position] != action.Val) {
        if (tape[position]) {
          ones--;
        }
        tape.Remove(position);
      }

      if (!tape.ContainsKey(position)) {
        tape.Add(position, action.Val);
        if (action.Val) {
          ones++;
        }
      }

      position += action.Right ? 1 : -1;
      state = states[action.NextState];
    }

    Console.WriteLine(ones);
  }
}
