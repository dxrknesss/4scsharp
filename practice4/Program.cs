namespace practice4;

class Program
{
  static void Main(string[] args)
  {
    // Pong.MessageStateHandler PongMessage = SendPongMessage;
    // Ping.MessageStateHandler PingMessage = SendPingMessage;
    // Ping.GetMessage("Pong", PingMessage);
    Rabbit r = new Rabbit(new Point(3, 4));
    r.ChangedLocation += (loc) => System.Console.WriteLine($"Lambda: Rabbit has changed location to: {loc}");
    Field f = new Field();
    DisplayField(f);
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
        System.Console.Write($"{f[i, j],5}");
      }
      System.Console.WriteLine();
    }
  }
}

