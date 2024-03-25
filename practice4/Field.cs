using System.Collections;
namespace practice4;

class Field
{
  static uint _xDimension = 18, _yDimension = 18;
  char[,] _field = new char[_xDimension, _yDimension];
  char[,] _fieldWithoutHeroes = new char[_xDimension, _yDimension];
  static Random _rnd = new Random();
  public static bool Debug = false;
  public delegate Point RandomMove(Point InPoint, Point RefPoint);
  public RandomMove HunterMoveStrategy;
  byte _carrotCount = 5;
  public Rabbit AttachedRabbit { get; }
  public Hunter AttachedHunter { get; }
  ArrayList _carrotPoints = new ArrayList();

  public char this[int row, int col] // indexer
  {
    get => _field[row, col];
    set => _field[row, col] = value;
  }

  public char this[Point p] // overloaded indexer, supports points as input
  {
    get => _field[p.X, p.Y];
    set => _field[p.X, p.Y] = value;
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

    for (int i = 0; i < _xDimension; i++)
    {
      for (int j = 0; j < _yDimension; j++)
      {
        _fieldWithoutHeroes[i, j] = GenerateDebris(false); // generate reference field
        if (i >= 2 && i <= _xDimension - 2
            && j >= 2 && j <= _yDimension - 2)
        {
          if (_rnd.Next(0, 100) < 20) _fieldWithoutHeroes[i, j] = GenerateDebris(true); // generate reference field
        }
      }
    }

    for (int i = 0; i < _xDimension; i++)
    {
      for (int j = 0; j < _yDimension; j++)
      {
        _field[i, j] = _fieldWithoutHeroes[i, j]; // copy all the data to field with heroes
      }
    }


    AttachedHunter.Location = GenerateRandomPointInBounds(2, 2);
    this[AttachedHunter.Location] = 'H';

    Point CarrotPoint;
    for (byte CarrotCount = 0; CarrotCount < _carrotCount; CarrotCount++)
    {
      CarrotPoint = GenerateRandomPointInBounds(1, 1);
      while (this[CarrotPoint] == 'H' || this[CarrotPoint] == '¡'
          || this._carrotPoints.Contains(CarrotPoint))
      {
        CarrotPoint = GenerateRandomPointInBounds(1, 1);
      }
      _carrotPoints.Add(CarrotPoint);
      this[CarrotPoint] = 'c';
    }

    HunterMoveStrategy = GenerateMoveDependOnCords;

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
    this[RabbitX, RabbitY] = 'r';
    AttachedRabbit.Location = new Point(RabbitX, RabbitY);
  }

  public void OutputCarrotPoints()
  {
    for (int i = 0; i < _carrotPoints.Count; i++)
    {
      System.Console.WriteLine($"Carrot {i + 1}: {_carrotPoints[i]}");
    }
    Thread.Sleep(5000);
  }

  public void ChangeRabbitLocation(Point NewLocation)
  {
    int NewX = NewLocation.X, NewY = NewLocation.Y;

    if (IsOutOfRange(new Point(NewX, NewY))
        || this[NewX, NewY] == '¡') // if out of bounds or there's tree - don't move
    {
      return;
    }

    if (this[NewX, NewY] == 'c')
    {
      AttachedRabbit.Carrots++;
      if (Field.Debug)
      {
        System.Console.WriteLine($"AttachedRabbit.Carrots: {AttachedRabbit.Carrots}");
        Thread.Sleep(1500);
      }
    }

    if (_carrotPoints.Contains(AttachedRabbit.Location))
    {
      _carrotPoints.Remove(AttachedRabbit.Location);
    }

    if (AttachedRabbit.Location.Equals(AttachedHunter.Location))
    {
      this[AttachedRabbit.Location] = 'H';
    }
    else
    {
      this[AttachedRabbit.Location] =
        this._fieldWithoutHeroes[AttachedRabbit.Location.X, AttachedRabbit.Location.Y];
    }

    this[NewX, NewY] = 'r';

    AttachedRabbit.Location = NewLocation;
    if (Field.Debug)
    {
      Thread.Sleep(1500);
    }
  }

  public void ChangeHuntersLocation()
  {
    Point NewLocation = HunterMoveStrategy(AttachedHunter.Location, AttachedRabbit.Location);

    if (this[NewLocation] == '¡')
    {
      byte attempts = 0;
      HunterMoveStrategy = GenerateRandomMove;
      while (this[NewLocation] == '¡' && attempts < 5) // if 5 attempts are unsuccessfull, give up
      {
        NewLocation = HunterMoveStrategy(AttachedHunter.Location, AttachedRabbit.Location);
        while (IsOutOfRange(NewLocation))
        {
          NewLocation = HunterMoveStrategy(AttachedHunter.Location, AttachedRabbit.Location);
        }
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
      HunterMoveStrategy = GenerateMoveDependOnCords;
    }

    if (Field.Debug)
    {
      System.Console.WriteLine($"Hunter's current location: {AttachedHunter.Location}");
      System.Console.WriteLine($"Hunter's new location: {NewLocation}");
      Thread.Sleep(1500);
    }

    if (_carrotPoints.Contains(AttachedHunter.Location))
    {
      this[AttachedHunter.Location] = 'c';
    }
    else
    {
      this[AttachedHunter.Location] =
        this._fieldWithoutHeroes[AttachedHunter.Location.X, AttachedHunter.Location.Y];
    }
    AttachedHunter.Location = NewLocation;
    Point RandPoint;
    if (IsOutOfRange(NewLocation)) // if hunter is going to go out of bounds, respawn him in a random place on the field
    {
      RandPoint = GenerateRandomPointInBounds(2, 2);
      this[RandPoint] = 'H';
      NewLocation.X = RandPoint.X;
      NewLocation.Y = RandPoint.Y;
      return;
    }
    this[AttachedHunter.Location] = 'H';
    if (Field.Debug)
    {
      System.Console.WriteLine("Sucessfully moved hunter's char");
      System.Console.WriteLine($"New char's cords are: {AttachedHunter.Location}");
      Thread.Sleep(1500);
    }
  }

  bool IsOutOfRange(Point Location)
  {
    return Location.X >= _xDimension || Location.X < 0
      || Location.Y >= _yDimension || Location.Y < 0;
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

  static Point GenerateRandomPointInBounds(int FromStart, int BeforeEnd)
  {
    return new Point(
        _rnd.Next(FromStart, (int)_xDimension - BeforeEnd),
        _rnd.Next(FromStart, (int)_yDimension - BeforeEnd)
    );
  }

  static char GenerateDebris(bool Tree)
  {
    if (Tree)
    {
      return '¡';
    }
    else
    {
      int UpperBound = 4;
      int Rnd = _rnd.Next(0, UpperBound * UpperBound);
      return "_wx."[Rnd % UpperBound];
    }
  }
}

