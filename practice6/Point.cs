using System;

namespace practice6
{
    internal class Point : ICloneable
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

        public object Clone()
        {
            return new Point(this._x, this._y);
        }
    }
}
