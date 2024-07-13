using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GridManager gridManager;

    void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridManager.NodeFromWorldPoint(startPos);
        Node targetNode = gridManager.NodeFromWorldPoint(targetPos);

        PriorityQueue<Node> openSet = new PriorityQueue<Node>(new NodeComparer());
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            closedSet.Add(currentNode);

            foreach (Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour);
                    }
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        if (dstX > dstY && dstX > dstZ)
            return 14 * Mathf.Min(dstY, dstZ) + 10 * (dstX - Mathf.Min(dstY, dstZ));
        else if (dstY > dstX && dstY > dstZ)
            return 14 * Mathf.Min(dstX, dstZ) + 10 * (dstY - Mathf.Min(dstX, dstZ));
        else
            return 14 * Mathf.Min(dstX, dstY) + 10 * (dstZ - Mathf.Min(dstX, dstY));
    }
}

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
