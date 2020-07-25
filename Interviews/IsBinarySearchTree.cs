// Дано бинарное дерево, проверить, является ли оно BST (бинарным деревом поиска)
// Узел дерева class Node { int Value; Node Left; Node Right }, реализовать IsBST(Node root) 

using System;
using System.Collections.Generic;

class Node {
    public int Value { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
}

static class RecursiveSolver {
    public static bool IsBST(Node root) {
        if (root == null)
            return true;
        return IsBST(root, int.MinValue, int.MaxValue);
    }

    static bool IsBST(Node node, int minValue, int maxValue) {
        if (node == null)
            return true;
        if (node.Value < minValue || node.Value > maxValue) 
            return false;
        return IsBST(node.Left, minValue, node.Value - 1) && IsBST(node.Right, node.Value + 1, maxValue);
    }
}

static class IterativeSolver {
    class NodeCondition {
        public NodeCondition(Node node, int minValue, int maxValue) {
            Node = node;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Node Node { get; private set; }
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
    }

    public static bool IsBST(Node root) {
        if (root == null)
            return true;
        Stack<NodeCondition> stack = new Stack<NodeCondition>();
        stack.Push(new NodeCondition(root, int.MinValue, int.MaxValue));
        while (stack.Count > 0) {
            NodeCondition item = stack.Pop();
            if (item.Node.Value < item.MinValue || item.Node.Value > item.MaxValue)
                return false;
            if (item.Node.Left != null)
                stack.Push(new NodeCondition(item.Node.Left, item.MinValue, item.Node.Value - 1));
            if (item.Node.Right != null)
                stack.Push(new NodeCondition(item.Node.Right, item.Node.Value + 1, item.MaxValue));
        }
        return true;
    }
}
