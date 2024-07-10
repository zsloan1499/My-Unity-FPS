using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public string enemyName = "Base";

    public float speed = 10f;

    public float damage = 1f;

    public float health = 100f;

    enum baseState{ IDLE, PATROL, ATTACK, DEAD}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
