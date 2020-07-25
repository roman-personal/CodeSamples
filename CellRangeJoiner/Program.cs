using System;
using System.Diagnostics;

namespace CellRangeJoiner {
    class Program {
        static void Main(string[] args) {
            const int w = 10;
            const int h = 10;
            CellArray field = new CellArray(w, h);
            field.SetRect(0, 0, w - 1, h - 1, true);
            for (int x = 0; x < w; x +=5) {
                for (int y = 0; y < h; y += 5) {
                    field.Set(x, y, false);
                }
            }
            //field.Set(3, 2, false);
            //field.Set(3, 3, false);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Joiner joiner = new Joiner();
            var result = joiner.Join(field);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine(result.Count);
            foreach (var rect in result)
                Console.WriteLine(rect.ToString());
            Console.ReadLine();
        }
    }
}
