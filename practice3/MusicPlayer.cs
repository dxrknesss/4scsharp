namespace practice3;

class MusicPlayer
{
  byte _soundLevel;

  public byte SoundLevel
  {
    get { return _soundLevel; }
    set { _soundLevel = value > 100 ? (byte)100 : value; }
  }
}

