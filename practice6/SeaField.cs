﻿namespace practice6
{
    internal class SeaField
    {
        public static byte _xDim = 7, _yDim = 7;
        char[,] _field = new char[_xDim, _yDim];

        public char this[byte x, byte y]
        {
            get => _field[x, y];
            set => _field[x, y] = value;
        }

        public bool IsOutOfRange(Point p)
        {
            return p.X < 0 || p.X >= _xDim || p.Y < 0 || p.Y >= _yDim;
        }

        public bool IsOutOfRange(Point[] ps)
        {
            foreach (Point p in ps)
            {
                if (IsOutOfRange(p))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
