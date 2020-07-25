using System;
using System.Collections.Generic;

namespace CellRangeJoiner {
    class Joiner {
        readonly Scanner scanner = new Scanner();
        readonly Dictionary<int, List<CellRect>> buckets = new Dictionary<int, List<CellRect>>();

        public List<CellRect> Join(CellArray table) {
            var byRows = JoinByRows(table);
            if (byRows.Count < 2)
                return byRows;
            var byColumns = JoinByColumns(table);
            var result = byColumns.Count < byRows.Count ? byColumns :  byRows;
            result.Sort();
            return result;
        }

        List<CellRect> JoinByRows(CellArray table) {
            buckets.Clear();
            foreach (var rect in scanner.ScanByRows(table)) {
                List<CellRect> bucket;
                if (!buckets.TryGetValue(rect.Left, out bucket)) {
                    bucket = new List<CellRect>();
                    buckets.Add(rect.Left, bucket);
                }
                bucket.Add(rect);
            }
            List<CellRect> result = new List<CellRect>();
            foreach (var bucket in buckets.Values) {
                for (int i = bucket.Count - 1; i > 0; i--) {
                    var current = bucket[i];
                    var prev = bucket[i - 1];
                    if (current.Right == prev.Right && current.Top == prev.Bottom + 1) {
                        current.Top--;
                        bucket.RemoveAt(i - 1);
                    }
                }
                result.AddRange(bucket);
            }
            return result;
        }

        List<CellRect> JoinByColumns(CellArray table) {
            buckets.Clear();
            foreach (var rect in scanner.ScanByColumns(table)) {
                List<CellRect> bucket;
                if (!buckets.TryGetValue(rect.Top, out bucket)) {
                    bucket = new List<CellRect>();
                    buckets.Add(rect.Top, bucket);
                }
                bucket.Add(rect);
            }
            List<CellRect> result = new List<CellRect>();
            foreach (var bucket in buckets.Values) {
                for (int i = bucket.Count - 1; i > 0; i--) {
                    var current = bucket[i];
                    var prev = bucket[i - 1];
                    if (current.Bottom == prev.Bottom && current.Left == prev.Right + 1) {
                        current.Left--;
                        bucket.RemoveAt(i - 1);
                    }
                }
                result.AddRange(bucket);
            }
            return result;
        }
    }
}
