// Дан односвязный список с узлом class Node { int Value; Node Next }
// Написать функцию Node RemoveLast(Node list, int value) удаляющую из списка последний узел со значением value 

using System;

class Node {
    public int Value { get; set; }
    public Node Next { get; set; }
}

class Program {
	static void Main(string[] args) {
		Node list = Create(new int[] { 1, 2, 3, 4, 3, 5 });
		Console.WriteLine("Initial");
		Print(list);
		list = RemoveLast(list, 3);
		Console.WriteLine("Result");
		Print(list);
		Console.ReadLine();
	}

	static Node RemoveLast(Node list, int value) {
		if (list == null)
			return null;
		Node precedingLast = null;
		Node previous = null;
		Node current = list;
		while (current != null) {
			if (current.Value == value)
				precedingLast = previous;
			previous = current;
			current = current.Next;
		}
		if (precedingLast != null)
			precedingLast.Next = precedingLast.Next.Next;
		else if (list.Value == value)
			return list.Next;
		return list;
	}
	
	static Node Create(int[] values) {
		Node result = null;
		for (int i = values.Length - 1; i >= 0; i--) {
			Node node = new Node();
			node.Value = values[i];
			node.Next = result;
			result = node;
		}
		return result;
	}

	static void Print(Node node) {
		if (node == null)
			Console.WriteLine("Empty");
		else {
			while (node != null) {
				Console.Write(node.Value);
				Console.Write(" ");
				node = node.Next;
			}
			Console.WriteLine();
		}
	}
}
