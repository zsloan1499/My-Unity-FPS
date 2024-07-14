using System.Collections.Generic;

public class NodeComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        int compare = x.fCost.CompareTo(y.fCost);
        if (compare == 0)
        {
            compare = x.hCost.CompareTo(y.hCost);
        }
        return compare;
    }
}
