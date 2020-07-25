// Дан массив целых чисел - нулей и единиц. Написать процедуру void order(int[] values)
// упорядочивающую значения таким образом чтобы нули оказались в начале массива, а единицы - в конце. 

using System;

class Program {
	static void Main(string[] args) {
		int[] values = new int[] { 0, 0, 1, 0, 0, 1, 1 };
		Console.WriteLine("Initial");
		Print(values);
		Order(values);
		Console.WriteLine("Ordered");
		Print(values);
		Console.ReadLine();
	}

	static void Order(int[] values) {
		int lastZeroPosition = -1;
		for (int i = 0; i < values.Length; i++) {
			if (values[i] == 0) {
				lastZeroPosition++;
				if (i > lastZeroPosition) {
					values[lastZeroPosition] = 0;
					values[i] = 1;
				}
			}
		}
	}

	static void Print(int[] values) {
		if (values == null || values.Length == 0)
			Console.WriteLine("Empty");
		else {
			for (int i = 0; i < values.Length; i++) {
				if (i != 0)
					Console.Write(" ");
				Console.Write(values[i]);
			}
			Console.WriteLine();
		}
	}
}
