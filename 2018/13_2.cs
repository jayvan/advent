using System;
using System.Collections.Generic;

public class Advent13_2 {
  public static void Main() {
    var track = new Dictionary<Tuple<int, int>, char>();
    var carts = new List<Cart>();
    string cartChars = "^>v<";
    string trackChars = "-|/\\+";

    string line;
    int y = 0;

    while ((line = Console.ReadLine()) != null) {
      for (int i = 0; i < line.Length; i++) {
        if (cartChars.IndexOf(line[i]) != -1) {
          carts.Add(new Cart(line[i], i, y));
        } else if (trackChars.IndexOf(line[i]) != -1) {
          track.Add(new Tuple<int, int>(i, y), line[i]);
        }
      }

      y++;
    }

    var cartLocations = new Dictionary<Tuple<int,int>, Cart>();
    foreach (Cart cart in carts) {
      cartLocations.Add(cart.Location, cart);
    }

    while (true) {
      foreach (Cart cart in carts) {
        if (cart.Dead) {
          continue;
        }

        var currentLocation = cart.Location;
        cart.Move(track);
        var newLocation = cart.Location;
        if (cartLocations.ContainsKey(newLocation)) {
          cart.Dead = true;
          cartLocations[newLocation].Dead = true;
          cartLocations.Remove(newLocation);
          cartLocations.Remove(currentLocation);
        } else {
          cartLocations.Remove(currentLocation);
          cartLocations.Add(newLocation, cart);
        }
      }

      carts.RemoveAll(cart => cart.Dead);

      if (carts.Count == 1) {
        Console.WriteLine(carts[0].Location);
        break;
      }

      carts.Sort();
    }
  }

  public class Cart : IComparable {
    enum Direction {
      Up, Right, Down, Left
    }

    enum Turn {
      Left, Straight, Right
    }

    private Turn nextTurn = Turn.Left;
    private Direction direction;
    public int X;
    public int Y;
    public bool Dead = false;

    public Tuple<int, int> Location => new Tuple<int, int>(this.X, this.Y);

    public Cart(char direction, int x, int y) {
      if (direction == '>') {
        this.direction = Direction.Right;
      } else if (direction == '<') {
        this.direction = Direction.Left;
      } else if (direction == '^') {
        this.direction = Direction.Up;
      } else {
        this.direction = Direction.Down;
      }

      this.X = x;
      this.Y = y;
    }

    public void Move(Dictionary<Tuple<int, int>, char> track) {
      switch (this.direction) {
        case Direction.Down:
          this.Y++;
          break;
        case Direction.Up:
          this.Y--;
          break;
        case Direction.Right:
          this.X++;
          break;
        case Direction.Left:
          this.X--;
          break;
      }

      var location = this.Location;
      if (track.ContainsKey(location)) {
        char trackPiece = track[location];

        if (trackPiece == '/') {
          if (this.direction == Direction.Up) {
            this.direction = Direction.Right;
          } else if (this.direction == Direction.Right) {
            this.direction = Direction.Up;
          } else if (this.direction == Direction.Down) {
            this.direction = Direction.Left;
          } else {
            this.direction = Direction.Down;
          }
        } else if (trackPiece == '\\') {
          if (this.direction == Direction.Up) {
            this.direction = Direction.Left;
          } else if (this.direction == Direction.Right) {
            this.direction = Direction.Down;
          } else if (this.direction == Direction.Down) {
            this.direction = Direction.Right;
          } else {
            this.direction = Direction.Up;
          }
        } else if (trackPiece == '+') {
          if (this.nextTurn == Turn.Left) {
            this.direction = (Direction)((int)(this.direction + 3) % 4);
          } else if (this.nextTurn == Turn.Right) {
            this.direction = (Direction)((int)(this.direction + 1) % 4);
          }

          this.nextTurn = (Turn)((int)(this.nextTurn + 1) % 3);
        }
      }
    }

    public int CompareTo(object obj) {
      Cart other = obj as Cart;

      int result = this.Y.CompareTo(other.Y);
      if (result == 0) {
        result = this.X.CompareTo(other.X);
      }

      return result;
    }
  }
}
