using System;

namespace CellRangeJoiner {
    class CellArray {
        readonly bool[] cells;

        public CellArray(int width, int height) {
            if (width < 1)
                throw new ArgumentException("width");
            if (height < 1)
                throw new ArgumentException("height");
            Width = width;
            Height = height;
            cells = new bool[width * height];
        }

        public int Width { get; }
        
        public int Height { get; }

        public bool Get(int x, int y) {
            CheckIndex(x, y);
            return cells[x + y * Width];
        }

        public void Set(int x, int y, bool value) {
            CheckIndex(x, y);
            cells[x + y * Width] = value;
        }

        public void SetRect(int left, int top, int right, int bottom, bool value) {
            CheckIndex(left, top);
            CheckIndex(right, bottom);
            for (int y = top; y <= bottom; y++) {
                int offset = y * Width;
                for (int x = left; x <= right; x++) {
                    cells[x + offset] = value;
                }
            }
        }

        void CheckIndex(int x, int y) {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("y");
        }
    }
}
