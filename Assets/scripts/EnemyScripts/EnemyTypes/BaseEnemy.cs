using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Transform target; // Player target
    public float speed = 2f; // Movement speed
    private List<Node> path; // Path to follow
    private GridManager gridManager;
    public float pathUpdateInterval = 0.1f; // Interval to update the enemy pathing
    public float nodeRadius = 0.1f; // Node radius for movement threshold

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure grid is initialized
        gridManager = FindObjectOfType<GridManager>();
        StartCoroutine(UpdatePathCoroutine()); // Start path recalculation coroutine
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveAlongPath(); // Continue moving along the path
        }
    }

    IEnumerator UpdatePathCoroutine()
    {
        while (true)
        {
            // Only recalculate the path if there is a target
            if (target != null)
            {
                FindPath(transform.position, target.position);
            }
            yield return new WaitForSeconds(pathUpdateInterval); // Recalculate path every 0.5 seconds
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridManager.NodeFromWorldPoint(startPos);
        Node targetNode = gridManager.NodeFromWorldPoint(targetPos);

        // If the target node is not walkable, find the nearest walkable node
        if (!targetNode.walkable)
        {
            targetNode = gridManager.GetNearestWalkableNode(targetNode);
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // If we reach the target node, retrace the path
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            // Check neighboring nodes
            foreach (Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue; // Skip if not walkable or already checked
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        Debug.Log("No path found!");
        path = null; // No path found
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        newPath.Reverse();
        path = newPath; // Set the new path for the enemy to follow
        Debug.Log($"Path found: {path.Count} nodes.");
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        // Calculate the cost based on grid movement
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY) + 10 * dstZ;
        return 14 * dstX + 10 * (dstY - dstX) + 10 * dstZ;
    }

    void MoveAlongPath()
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("Path is null or empty");
            return;
        }

        Node targetNode = path[0];
        Vector3 targetPosition = targetNode.worldPosition;
        targetPosition.y = transform.position.y; // Maintain the same height

        if (Vector3.Distance(transform.position, targetPosition) < nodeRadius)
        {
            path.RemoveAt(0); // Remove the node if close enough
        }
        else
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 move = new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;

            // Move towards the target node, checking for obstacles
            if (!Physics.Raycast(transform.position, direction, move.magnitude))
            {
                transform.Translate(move, Space.World); // Move if no collision
            }
            else
            {
                Debug.Log("Collision detected, recalculating path...");
                // Recalculate the path if a collision is detected
                FindPath(transform.position, target.position);
            }
        }
    }
}
