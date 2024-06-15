using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pistol : Gun
{
    public float aimTime;

    // Override Shoot method if specific to pistols
    public override void Shoot()
    {
        base.Shoot();
        // Additional pistol-specific shooting logic
        Debug.Log("Pistol shooting logic executed.");
    }
}
