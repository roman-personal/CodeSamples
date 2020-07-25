using System;
using System.Collections.Generic;

namespace CellRangeJoiner {
    class Scanner {
        public IEnumerable<CellRect> ScanByRows(CellArray table) {
            bool flag;
            int start = 0;
            for (int y = 0; y < table.Height; y++) {
                flag = false;
                for (int x = 0; x < table.Width; x++) {
                    if (flag) {
                        if (!table.Get(x, y)) {
                            flag = false;
                            yield return new CellRect(start, y, x - 1, y);
                        }
                    }
                    else {
                        if (table.Get(x, y)) {
                            flag = true;
                            start = x;
                        }
                    }
                }
                if (flag)
                    yield return new CellRect(start, y, table.Width - 1, y);
            }
        }

        public IEnumerable<CellRect> ScanByColumns(CellArray table) {
            bool flag;
            int start = 0;
            for (int x = 0; x < table.Width; x++) {
                flag = false;
                for (int y = 0; y < table.Height; y++) {
                    if (flag) {
                        if (!table.Get(x, y)) {
                            flag = false;
                            yield return new CellRect(x, start, x, y - 1);
                        }
                    }
                    else {
                        if (table.Get(x, y)) {
                            flag = true;
                            start = y;
                        }
                    }
                }
                if (flag)
                    yield return new CellRect(x, start, x, table.Height - 1);
            }
        }
    }
}
