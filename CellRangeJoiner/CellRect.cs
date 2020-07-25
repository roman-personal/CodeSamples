using System;

namespace CellRangeJoiner {
    class CellRect : IComparable<CellRect> {
        public CellRect() {
            Left = 0;
            Top = 0;
            Right = 0;
            Bottom = 0;
        }

        public CellRect(int left, int top, int right, int bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int Width => Right - Left + 1;

        public int Height => Bottom - Top + 1;

        public int CompareTo(CellRect other) {
            if (other == null)
                return -1;
            int result = Left.CompareTo(other.Left);
            if (result == 0)
                result = Top.CompareTo(other.Top);
            return result;
        }

        public override string ToString() => $"({Left}, {Top}), ({Right}, {Bottom})";
    }
}
