namespace practice4;

class Field
{
  private char[,] _field = new char[7, 7];

  public char this[int row, int col] // indexer
  {
    get => _field[row, col];
    set => _field[row, col] = value;
  }

  public char[,] FieldGrid
  {
    get => _field;
  }

  public Field()
  {
    for (int i = 0; i < _field.GetLength(0); i++)
    {
      for (int j = 0; j < _field.GetLength(1); j++)
      {
        _field[i, j] = '0';
      }
    }
  }
}

