using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 position;
    public int gridX;
    public int gridY;
    public int gridZ;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node(Vector3 _position, bool _walkable)
    {
        position = _position;
        walkable = _walkable;
    }
}
