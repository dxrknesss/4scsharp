namespace practice4;

class Ping
{
  public delegate void MessageStateHandler();

  public static void GetMessage(string message, MessageStateHandler? handler)
  {
    if (message.Equals("Pong"))
    {
      if (handler != null) handler();
    }
  }
}

