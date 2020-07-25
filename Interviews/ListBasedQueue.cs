// Реализовать очередь на базе односвязного списка с узлом class Node { Object Value; Node Next; }
// Должны быть реализованы методы bool IsEmpty(); void Enqueue(Object value); Object Dequeue(); 

using System;

class Node {
    public Object Value { get; set; }
    public Node Next { get; set; }
}

class Queue {
    Node head = null;
    Node tail = null;

    public bool IsEmpty() => head == null;

    public void Enqueue(Object value) {
        Node node = new Node();
        node.Value = value;
        if (IsEmpty()) {
            head = node;
            tail = node;
        }
        else {
            tail.Next = node;
            tail = node;
        }
    }

    public Object Dequeue() {
        if (IsEmpty())
            throw new InvalidOperationException();
        Object result = head.Value;
        head = head.Next;
        if (head == null)
            tail = null;
        return result;
    }
}
