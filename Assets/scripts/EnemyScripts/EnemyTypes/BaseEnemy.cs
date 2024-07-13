using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public string enemyName = "Base";
    public float speed = 10f;
    public float damage = 1f;
    public float health = 100f;

    enum baseState { IDLE, PATROL, ATTACK, DEAD }
    baseState currentState = baseState.IDLE;

    Transform target;
    List<Node> path;
    int targetIndex;
    Pathfinding pathfinding;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        target = GameObject.FindWithTag("Player").transform;
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (currentState == baseState.ATTACK)
        {
            MoveAlongPath();
        }
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (pathfinding != null && target != null)
            {
                path = pathfinding.FindPath(transform.position, target.position);
                if (path == null || path.Count == 0)
                {
                    Debug.Log("No valid path found.");
                }
                else
                {
                    Debug.Log("Path updated. Path length: " + path.Count);
                }
                targetIndex = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void MoveAlongPath()
    {
        if (path == null || path.Count == 0 || targetIndex >= path.Count)
        {
            Debug.Log("No valid path or target index out of range.");
            return;
        }

        Node targetNode = path[targetIndex];
        Vector3 targetPosition = targetNode.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            targetIndex++;
            if (targetIndex >= path.Count)
            {
                currentState = baseState.IDLE;
                Debug.Log("Reached end of path. Changing state to IDLE.");
            }
        }
    }
}
