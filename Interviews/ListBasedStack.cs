// Реализовать стек на базе односвязного списка с узлом class Node { Object Value; Node Next; }
// Должны быть реализованы методы bool IsEmpty(); void Push(Object value); Object Pop(); 

using System;

class Node {
    public Object Value { get; set; }
    public Node Next { get; set; }
}

class Stack {
    Node head = null;

    public bool IsEmpty() => head == null;

    public void Push(Object value) {
        Node node = new Node();
        node.Value = value;
        node.Next = head;
        head = node;
    }

    public Object Pop() {
        if (IsEmpty())
            throw new InvalidOperationException();
        Object result = head.Value;
        head = head.Next;
        return result;
    }
}
