namespace practice4;

class Field
{
  static uint _xDimension = 7, _yDimension = 7;
  char[,] _field = new char[_xDimension, _yDimension];
  char[,] _fieldWithoutHeroes = new char[_xDimension, _yDimension];
  static Random _rnd = new Random();

  public char this[int row, int col] // indexer
  {
    get => _field[row, col];
    set => _field[row, col] = value;
  }

  public char[,] FieldGrid
  {
    get => _field;
  }

  public Field(Hunter h, Rabbit r)
  {
    for (int i = 0; i < _field.GetLength(0); i++)
    {
      for (int j = 0; j < _field.GetLength(1); j++)
      {
        _fieldWithoutHeroes[i, j] = GenerateDebris(); // generate reference field
      }
    }

    for (int i = 0; i < _field.GetLength(0); i++)
    {
      for (int j = 0; j < _field.GetLength(1); j++)
      {
        _field[i, j] = _fieldWithoutHeroes[i, j]; // copy all the data to field with heroes
      }
    }

    int X = _rnd.Next(2, _field.GetLength(0) - 2);
    int Y = _rnd.Next(2, _field.GetLength(1) - 2);
    _field[X, Y] = 'H';
    h.Location = new Point(X, Y);

    switch (_rnd.Next(0, 4))
    {
      case 0:
        _field[0, 0] = 'r';
        r.Location = new Point(0, 0);
        break;
      case 1:
        _field[_field.GetLength(0) - 1, 0] = 'r';
        r.Location = new Point(_field.GetLength(0) - 1, 0);
        break;
      case 2:
        _field[0, _field.GetLength(1) - 1] = 'r';
        r.Location = new Point(0, _field.GetLength(1) - 1);
        break;
      case 3:
        _field[_field.GetLength(0) - 1, _field.GetLength(1) - 1] = 'r';
        r.Location = new Point(_field.GetLength(0) - 1, _field.GetLength(1) - 1);
        break;
    }
  }

  public void ChangeRabbitPosition(Point NewPosition, Rabbit r) // TODO: create exceptions for out of bounds
  {
    this._field[r.Location.X, r.Location.Y] = this._fieldWithoutHeroes[r.Location.X, r.Location.Y];
    int NewX = NewPosition.X, NewY = NewPosition.Y;

    if (this._field[NewX, NewY] == '¡')
    {
      this._field[r.Location.X, r.Location.Y] = 'r';
      return;
    }

    if (NewX >= _xDimension)
    {
      NewX = (int)_xDimension - 1;
    }
    else if (NewX < 0)
    {
      NewX = 0;
    }

    if (NewY >= _yDimension)
    {
      NewY = (int)_yDimension - 1;
    }
    else if (NewY < 0)
    {
      NewY = 0;
    }

    this._field[NewX, NewY] = 'r';

    r.Location = new Point(NewX, NewY);
  }

  public void ChangeHuntersLocation(Hunter h, Rabbit r)
  {
    int NewX = h.Location.X, NewY = h.Location.Y;
    this._field[h.Location.X, h.Location.Y] = this._fieldWithoutHeroes[h.Location.X, h.Location.Y];
    switch (_rnd.Next(0, 2))
    {
      case 0:
        if (h.Location.X < r.Location.X)
        {
          NewX++;
        }
        else
        {
          NewX--;
        }
        break;
      case 1:
        if (h.Location.Y < r.Location.Y)
        {
          NewY++;
        }
        else
        {
          NewY--;
        }
        break;
    }

    if (this._field[NewX, NewY] == '¡')
    {
      this._field[h.Location.X, h.Location.Y] = 'H';
      return;
    }

    if (NewX >= _xDimension || NewX < 0
        || NewY >= _yDimension || NewY < 0) // if hunter is going to go out of bounds, don't do anything
    {
      int randX = _rnd.Next(2, (int)_xDimension - 2), randY = _rnd.Next(2, (int)_yDimension - 2);
      this._field[randX, randY] = 'H';
      NewX = randX;
      NewY = randY;
      return;
    }
    this._field[NewX, NewY] = 'H';
  }

  static char GenerateDebris()
  {
    int UpperBound = 5;
    int Rnd = _rnd.Next(0, UpperBound);
    return "_wx.¡"[Rnd % UpperBound];
  }
}

