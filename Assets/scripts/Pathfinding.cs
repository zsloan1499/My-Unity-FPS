using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    GridManager gridManager;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        Debug.Log("Pathfinding Start method called");
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridManager.NodeFromWorldPoint(startPos);
        Node targetNode = gridManager.NodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null)
        {
            Debug.Log($"Start node or target node is null. StartNode: {startNode}, TargetNode: {targetNode}");
            return null;
        }

        PriorityQueue<Node> openSet = new PriorityQueue<Node>(new NodeComparer());
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Enqueue(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();

            if (currentNode == targetNode)
            {
                Debug.Log("Path found");
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

        Debug.Log("Path not found");
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
        Debug.Log("Retracing path. Path length: " + path.Count);
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
