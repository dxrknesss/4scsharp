namespace practice4;

class Program
{
  delegate void SetInterval(int IntervalSeconds, Action action);

  static void Main(string[] args)
  {
    // ---------- TASK 1 ----------
    // Pong.MessageStateHandler PongMessage = SendPongMessage;
    // Ping.MessageStateHandler PingMessage = SendPingMessage;
    // Ping.GetMessage("Pong", PingMessage);

    // ---------- TASK 2 ----------
    Task2();
  }

  static void Task2()
  {
    Rabbit r = new Rabbit();
    Hunter h = new Hunter();
    Field f = new Field(h, r);

    r.ChangedLocation += (Loc, OldLoc) =>
      System.Console.WriteLine($"Lambda: Rabbit has changed location to: {Loc}");
    r.ChangedLocation += delegate (Point Loc, Point NewLoc)
    {
      System.Console.WriteLine($"Anonymous: Rabbit has changed location to: {Loc}");
    };
    r.ChangedLocation += Hunter.OutputRabbitLocation;

    while (!r.Location.Equals(h.Location) && r.Carrots < f.CarrotCount)
    {
      Console.Clear();
      DisplayField(f);
      var key = Console.ReadKey();
      // create new point, so old won't be changed because of the same reference
      var location = new Point(r.Location.X, r.Location.Y);
      switch (key.Key)
      {
        case ConsoleKey.UpArrow:
          location.X--;
          break;
        case ConsoleKey.DownArrow:
          location.X++;
          break;
        case ConsoleKey.LeftArrow:
          location.Y--;
          break;
        case ConsoleKey.RightArrow:
          location.Y++;
          break;
        case ConsoleKey.D:
          Field.Debug = !Field.Debug;
          continue;
        case ConsoleKey.R:
          Task2();
          return;
        default:
          continue;
      }
      if (location.Equals(h.Location))
      {
        f.ChangeHuntersLocation(h, r);
        break;
      }
      f.ChangeHuntersLocation(h, r);

      f.ChangeRabbitLocation(location, r);
      if (r.Location.Equals(h.Location))
      {
        f.ChangeRabbitLocation(location, r);
        break;
      }
    }
    Console.Clear();
    f[h.Location.X, h.Location.Y] = 'H';
    DisplayField(f);

    Console.ForegroundColor = ConsoleColor.White;
    if (r.Carrots == f.CarrotCount)
    {
      Console.BackgroundColor = ConsoleColor.Green;
      System.Console.WriteLine("You won!");
    }
    else
    {
      Console.BackgroundColor = ConsoleColor.Red;
      System.Console.WriteLine("You have been caught!");
    }

    Console.BackgroundColor = ConsoleColor.Blue;
    System.Console.WriteLine("Restart? Press 'R'");
    if (Console.ReadKey().Key.Equals(ConsoleKey.R))
    {
      Console.ResetColor();
      Task2();
    }
  }

  static void SendPongMessage()
  {
    Thread.Sleep(1000);
    System.Console.WriteLine("Pong");
    Ping.GetMessage("Pong", SendPingMessage);
  }

  static void SendPingMessage()
  {
    Thread.Sleep(1000);
    System.Console.WriteLine("Ping");
    Pong.GetMessage("Ping", SendPongMessage);
  }

  static void DisplayField(Field f)
  {
    for (int i = 0; i < f.FieldGrid.GetLength(0); i++)
    {
      for (int j = 0; j < f.FieldGrid.GetLength(1); j++)
      {
        if (f[i, j] == 'r') Console.ForegroundColor = ConsoleColor.Blue;
        if (f[i, j] == 'H') Console.ForegroundColor = ConsoleColor.Red;
        if (f[i, j] == '¡') Console.ForegroundColor = ConsoleColor.Green;
        if (f[i, j] == 'c') Console.ForegroundColor = ConsoleColor.Magenta;
        System.Console.Write($"{f[i, j],5}");
        Console.ResetColor();
      }
      System.Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.Magenta;
    System.Console.WriteLine($"Carrots left: {f.CarrotCount - f.AttachedRabbit.Carrots}");
    Console.ResetColor();
  }
}

