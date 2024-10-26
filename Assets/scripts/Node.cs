using UnityEngine;

[System.Serializable]
public class Node
{
    public bool walkable; // Is this node walkable?
    public Vector3 worldPosition; // World position of the node
    public int gridX; // X index in the grid
    public int gridY; // Y index in the grid
    public int gridZ; // Z index in the grid

    public int gCost; // Cost from the start node
    public int hCost; // Heuristic cost to the target node
    public Node parent; // Parent node in the path

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY, int gridZ)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        this.gridZ = gridZ;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }
}
