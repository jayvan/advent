using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Advent10 {
  interface Input {
    int Value { get; }
  }

  class BinInput : Input {
    readonly int binValue;

    public BinInput(int binValue) {
      this.binValue = binValue;
    }

    public int Value {
      get {
        return binValue;
      }
    }
  }

  class BotInput : Input {
    public enum OutputType { HIGH, LOW };

    readonly Bot inputBot;
    OutputType outputType;

    public BotInput(Bot inputBot, OutputType outputType) {
      this.inputBot = inputBot;
      this.outputType = outputType;
    }

    public int Value {
      get {
        if (outputType== OutputType.HIGH) {
          return inputBot.HighOutput;
        } else {
          return inputBot.LowOutput;
        }
      }
    }
  }

  class Bot {
    Input left;
    Input right;
    bool resultCached;
    int highOutput;
    int lowOutput;

    // Part 1 asks to find the robot which compares values 61 and 17
    public bool SatisfiesPartOne {
      get {
        var inputs = new int[2];
        inputs[0] = left.Value;
        inputs[1] = right.Value;
        Array.Sort(inputs);
        return inputs[0] == 17 && inputs[1] == 61;
      }
    }

    public int LowOutput {
      get {
        if (!resultCached) {
          CalculateOutputs();
        }

        return lowOutput;
      }
    }

    public int HighOutput {
      get {
        if (!resultCached) {
          CalculateOutputs();
        }

        return highOutput;
      }
    }

    public void AssignInput(Input input) {
      if (left == null) {
        left = input;
      } else {
        right = input;
      }
    }

    private void CalculateOutputs() {
      int leftInput = left.Value;
      int rightInput = right.Value;
      resultCached = true;

      if (leftInput < rightInput) {
        lowOutput = leftInput;
        highOutput = rightInput;
      } else {
        lowOutput = rightInput;
        highOutput = leftInput;
      }

    }
  }

  public class BotNet {
    static Regex constAssignment = new Regex("value ([0-9]+) goes to bot ([0-9]+)");
    static Regex botAssignment = new Regex("bot ([0-9]+) gives low to (bot|output) ([0-9]+) and high to (bot|output) ([0-9]+)");

    readonly Dictionary<int, Bot> botNetwork = new Dictionary<int, Bot>();
    readonly Dictionary<int, BotInput> outputBins = new Dictionary<int, BotInput>();

    public void AddConnection(string connection) {
      int botId;

      Match match = constAssignment.Match(connection);
      if (match.Success) {
        int binValue = int.Parse(match.Groups[1].Captures[0].ToString());
        botId = int.Parse(match.Groups[2].Captures[0].ToString());
        var binInput = new BinInput(binValue);
        GetBot(botId).AssignInput(binInput);
        return;
      }

      match = botAssignment.Match(connection);
      botId = int.Parse(match.Groups[1].Captures[0].ToString());
      bool lowIsOutput = match.Groups[2].Captures[0].ToString() == "output";
      int lowOutputId = int.Parse(match.Groups[3].Captures[0].ToString());
      var lowBotInput = new BotInput(GetBot(botId), BotInput.OutputType.LOW);
      bool highIsOutput = match.Groups[4].Captures[0].ToString() == "output";
      int highOutputId = int.Parse(match.Groups[5].Captures[0].ToString());
      var highBotInput = new BotInput(GetBot(botId), BotInput.OutputType.HIGH);

      if (lowIsOutput) {
        outputBins.Add(lowOutputId, lowBotInput);
      } else {
        GetBot(lowOutputId).AssignInput(lowBotInput);
      }

      if (highIsOutput) {
        outputBins.Add(highOutputId, highBotInput);
      } else {
        GetBot(highOutputId).AssignInput(highBotInput);
      }
    }

    public int PartOneBotId() {
      foreach (int robotId in botNetwork.Keys) {
        if (GetBot(robotId).SatisfiesPartOne) {
          return robotId;
        }
      }

      return -1;
    }

    public int PartTwoProduct() {
      int outputProduct = 1;

      for (int i = 0; i < 3; i++) {
        outputProduct *= outputBins[i].Value;
      }

      return outputProduct;
    }

    private Bot GetBot(int botId) {
      if (!botNetwork.ContainsKey(botId)) {
        Bot bot = new Bot();
        botNetwork.Add(botId, bot);
        return bot;
      }

      return botNetwork[botId];
    }
  }

  public static void Main () {
    var botNet = new BotNet();

    string line;
    while ((line = Console.ReadLine()) != null) {
      botNet.AddConnection(line);
    }

    Console.WriteLine("Part One: " + botNet.PartOneBotId());
    Console.WriteLine("Part Two: " + botNet.PartTwoProduct());
  }
}
