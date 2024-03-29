using System;

namespace practice6
{
    internal class Point : ICloneable
    {
        uint _x;
        uint _y;

        public uint X
        {
            get => _x;
            set => _x = value;
        }

        public uint Y
        {
            get => _y;
            set => _y = value;
        }

        public Point(uint x, uint y)
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
