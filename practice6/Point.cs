using System;

namespace practice6
{
    internal class Point
    {
        byte _x;
        byte _y;

        public byte X
        {
            get => _x;
            set => _x = value;
        }

        public byte Y
        {
            get => _y;
            set => _y = value;
        }

        public Point(byte x, byte y)
        {
            this._x = x;
            this._y = y;
        }

        public override string ToString()
        {
            return $"{_x}, {_y}";
        }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   _x == point._x &&
                   _y == point._y;
        }

        public override int GetHashCode()
        {
            int hashCode = 979593255;
            hashCode = hashCode * -1521134295 + _x.GetHashCode();
            hashCode = hashCode * -1521134295 + _y.GetHashCode();
            return hashCode;
        }
    }
}
