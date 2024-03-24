namespace practice4;

class Field
{
  static uint _xDimension = 18, _yDimension = 18;
  char[,] _field = new char[_xDimension, _yDimension];
  char[,] _fieldWithoutHeroes = new char[_xDimension, _yDimension];
  static Random _rnd = new Random();
  public static bool Debug = false;
  public delegate Point RandomMove(Point InPoint, Point RefPoint);
  public RandomMove MoveStrategy;
  byte _carrotCount = 5;
  public Rabbit AttachedRabbit { get; }
  public Hunter AttachedHunter { get; }

  public char this[int row, int col] // indexer
  {
    get => _field[row, col];
    set => _field[row, col] = value;
  }

  public byte CarrotCount
  {
    get => _carrotCount;
  }

  public char[,] FieldGrid
  {
    get => _field;
  }

  public Field(Hunter h, Rabbit r)
  {
    AttachedRabbit = r;
    AttachedHunter = h;

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


    AttachedHunter.Location = new Point(
        _rnd.Next(2, (int)_xDimension - 2),
        _rnd.Next(2, (int)_yDimension - 2));
    _field[AttachedHunter.Location.X, AttachedHunter.Location.Y] = 'H';

    int x, y;
    for (byte CarrotCount = 0; CarrotCount < _carrotCount; CarrotCount++)
    {
      x = _rnd.Next(1, (int)_xDimension - 1);
      y = _rnd.Next(1, (int)_yDimension - 1);
      while (_field[x, y] == 'H' || _field[x, y] == '¡')
      {
        x = _rnd.Next(1, (int)_xDimension - 1);
        y = _rnd.Next(1, (int)_yDimension - 1);
      }
      _field[x, y] = 'c';
    }

    MoveStrategy = GenerateMoveDependOnCords;

    int RabbitX = (int)_xDimension - 1, RabbitY = (int)_yDimension - 1;
    switch (_rnd.Next(0, 4)) // choose rabbit's position from 4 corners of a map
    {
      case 0: // 00
        RabbitX &= 0;
        RabbitY &= 0;
        break;
      case 1: // 01
        RabbitX &= 0;
        break;
      case 2: // 10
        RabbitY &= 0;
        break;
      case 3: // 11
        break;
    }
    this._field[RabbitX, RabbitY] = 'r';
    AttachedRabbit.Location = new Point(RabbitX, RabbitY);
  }

  public void ChangeRabbitLocation(Point NewLocation)
  {
    int NewX = NewLocation.X, NewY = NewLocation.Y;

    if (NewX >= _xDimension || NewX < 0
        || NewY >= _yDimension || NewY < 0
        || this._field[NewX, NewY] == '¡') // if out of bounds or there's tree - don't move
    {
      return;
    }

    if (this._field[NewX, NewY] == 'c')
    {
      AttachedRabbit.Carrots++;
      if (Field.Debug)
      {
        System.Console.WriteLine($"AttachedRabbit.Carrots: {AttachedRabbit.Carrots}");
        Thread.Sleep(1500);
      }
    }

    if (AttachedRabbit.Location.Equals(AttachedHunter.Location))
    {
      this._field[AttachedRabbit.Location.X, AttachedRabbit.Location.Y] = 'H';
    }
    else
    {
      this._field[AttachedRabbit.Location.X, AttachedRabbit.Location.Y] =
        this._fieldWithoutHeroes[AttachedRabbit.Location.X, AttachedRabbit.Location.Y];
    }

    this._field[NewX, NewY] = 'r';

    AttachedRabbit.Location = NewLocation;
    if (Field.Debug)
    {
      Thread.Sleep(1500);
    }
  }

  public void ChangeHuntersLocation()
  {
    Point NewLocation = MoveStrategy(AttachedHunter.Location, AttachedRabbit.Location);

    if (this._field[NewLocation.X, NewLocation.Y] == '¡')
    {
      byte attempts = 0;
      MoveStrategy = GenerateRandomMove;
      while (this._field[NewLocation.X, NewLocation.Y] == '¡' && attempts < 5) // if 5 attempts are unsuccessfull, give up
      {
        NewLocation = MoveStrategy(AttachedHunter.Location, AttachedRabbit.Location);
        attempts++;
      }

      if (attempts > 5)
      {
        if (Field.Debug)
        {
          System.Console.WriteLine($"Hunter didn't succed in finding new location: {NewLocation}");
          Thread.Sleep(1500);
        }
        return;
      }
      MoveStrategy = GenerateMoveDependOnCords;
    }

    if (Field.Debug)
    {
      System.Console.WriteLine($"Hunter's current location: {AttachedHunter.Location}");
      System.Console.WriteLine($"Hunter's new location: {NewLocation}");
      Thread.Sleep(1500);
    }

    this._field[AttachedHunter.Location.X, AttachedHunter.Location.Y] =
      this._fieldWithoutHeroes[AttachedHunter.Location.X, AttachedHunter.Location.Y];
    AttachedHunter.Location = NewLocation;
    if (NewLocation.X >= _xDimension || NewLocation.X < 0
        || NewLocation.Y >= _yDimension || NewLocation.Y < 0) // if hunter is going to go out of bounds, respawn him in a random place on the field
    {
      int randX = _rnd.Next(2, (int)_xDimension - 2), randY = _rnd.Next(2, (int)_yDimension - 2);
      this._field[randX, randY] = 'H';
      NewLocation.X = randX;
      NewLocation.Y = randY;
      return;
    }
    this._field[AttachedHunter.Location.X, AttachedHunter.Location.Y] = 'H';
    if (Field.Debug)
    {
      System.Console.WriteLine("Sucessfully moved hunter's char");
      System.Console.WriteLine($"New char's cords are: {AttachedHunter.Location}");
      Thread.Sleep(1500);
    }
  }

  Point GenerateMoveDependOnCords(Point InPoint, Point RefPoint)
  {
    Point OutPoint = new Point(InPoint.X, InPoint.Y);
    if (Math.Abs(InPoint.X - RefPoint.X) > Math.Abs(InPoint.Y - RefPoint.Y)) // if distance by X coordinate is bigger, then move by X, otherwise move by Y
    {
      System.Console.WriteLine("Distance by X is bigger than by Y, moving by X");
      if (InPoint.X < RefPoint.X)
      {
        OutPoint.X++;
      }
      else
      {
        OutPoint.X--;
      }
    }
    else
    {
      System.Console.WriteLine("Distance by Y is bigger than by X, moving by Y");
      if (InPoint.Y < RefPoint.Y)
      {
        OutPoint.Y++;
      }
      else
      {
        OutPoint.Y--;
      }
    }

    return OutPoint;
  }

  Point GenerateRandomMove(Point InPoint, Point RefPoint) // generates random movement for hunter based on rabbit's location
  {
    Point OutPoint = new Point(InPoint.X, InPoint.Y);
    switch (_rnd.Next(0, 4))
    {
      case 0:
        OutPoint.X++;
        break;
      case 1:
        OutPoint.X--;
        break;
      case 2:
        OutPoint.Y++;
        break;
      case 3:
        OutPoint.Y--;
        break;
    }

    return OutPoint;
  }

  static char GenerateDebris()
  {
    int UpperBound = 4;
    int Rnd = _rnd.Next(0, UpperBound * UpperBound);
    if (Rnd <= UpperBound * 0.5) return '¡';
    return "_wx."[Rnd % UpperBound];
  }
}

