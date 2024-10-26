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

        List<Node> openSet = new List<Node>(); // Nodes to be evaluated
        HashSet<Node> closedSet = new HashSet<Node>(); // Nodes already evaluated
        openSet.Add(startNode); // Start pathfinding from the start node

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode); // Return the path once the target node is reached
            }

            foreach (Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue; // Skip unwalkable nodes or those already evaluated
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        Debug.Log("Path not found");
        return null; // Return null if no path is found
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
        return path; // Return the retraced path
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
