using System.Collections.Generic;

namespace practice6
{
    internal class SeaField
    {
        public static byte _xDim = 7, _yDim = 7;
        char[,] _field = new char[_xDim, _yDim];
        public List<Point> ShipPoints;

        public SeaField()
        {
            ShipPoints = new List<Point>();
        }

        public char this[byte x, byte y]
        {
            get => _field[x, y];
            set => _field[x, y] = value;
        }

        public char this[Point[] ps]
        {
            set
            {
                foreach(Point p in ps)
                {
                    _field[p.X, p.Y] = value;
                }
            }
        }

        public bool IsValidPoint(Point p)
        {
            return p != null && p.X >= 0 && p.X < _xDim && p.Y >= 0 && p.Y < _yDim 
                && this[p.X, p.Y] != 's' && this[p.X, p.Y] != 'x';
        }

        public bool IsValidPoint(Point[] ps)
        {
            if (ps.Length == 0)
            {
                return false;
            }
            foreach (Point p in ps)
            {
                if (!IsValidPoint(p))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
