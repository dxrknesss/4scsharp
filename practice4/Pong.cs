namespace practice4;

class Pong
{
  public delegate void MessageStateHandler();

  public static void GetMessage(string message, MessageStateHandler? handler)
  {
    if (message.Equals("Ping"))
    {
      if (handler != null) handler();
    }
  }
}

