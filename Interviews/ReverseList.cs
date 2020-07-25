// Дан односвязный список с узлом class Node { int Value; Node Next } 
// Написать функцию Node Reverse(Node head) меняющую порядок узлов на обратный
// Исходный список можно разрушить 

using System;

class Node {
    public int Value { get; set; }
    public Node Next { get; set; }
}

class Progam
{
	static void Main(String[] args)
	{
        Node list = Create(new int[] { 1, 2, 3});
        Console.WriteLine("Initial");
        Print(list);
        list = Reverse(list);
        Console.WriteLine("Result");
        Print(list);
        Console.ReadLine();
	}

    static Node Reverse(Node node) {
        Node result = null;
        while (node != null) {
            Node temp = node.Next;
            node.Next = result;
            result = node;
            node = temp;
        }
        return result;
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