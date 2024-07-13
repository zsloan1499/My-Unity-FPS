using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public UnityEngine.Vector3 gridWorldSize;
    public float nodeRadius;
    Node[,,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        UnityEngine.Vector3 worldBottomLeft = transform.position - UnityEngine.Vector3.right * gridWorldSize.x / 2 - UnityEngine.Vector3.forward * gridWorldSize.z / 2 - UnityEngine.Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    UnityEngine.Vector3 worldPoint = worldBottomLeft + UnityEngine.Vector3.right * (x * nodeDiameter + nodeRadius) + UnityEngine.Vector3.up * (y * nodeDiameter + nodeRadius) + UnityEngine.Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, LayerMask.GetMask("Obstacle")));
                    grid[x, y, z] = new Node(worldPoint, walkable);
                }
            }
        }
    }

    public Node NodeFromWorldPoint(UnityEngine.Vector3 worldPosition)
    {
        // Calculate grid indices from world position
        int x = Mathf.FloorToInt((worldPosition.x + gridWorldSize.x / 2) / nodeDiameter);
        int y = Mathf.FloorToInt((worldPosition.y + gridWorldSize.y / 2) / nodeDiameter);
        int z = Mathf.FloorToInt((worldPosition.z + gridWorldSize.z / 2) / nodeDiameter);

        // Check if indices are within bounds of the grid
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && z >= 0 && z < gridSizeZ)
        {
            return grid[x, y, z];
        }
        else
        {
            return null;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && y == 0 && z == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY && checkZ >= 0 && checkZ < gridSizeZ)
                    {
                        neighbours.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new UnityEngine.Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.position, UnityEngine.Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
