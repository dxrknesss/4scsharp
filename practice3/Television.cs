namespace practice3;

class Television 
{
  uint _currentChannel;

  public void NextChannel() 
  {
    this._currentChannel++;
  }

  public void PreviousChannel() 
  {
    this._currentChannel--;
  }

  public void SetChannel(uint number)
  {
    this._currentChannel = number;
  }
}

