using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deagle : Pistol
{
    public Deagle()
    {
        gunName = "Deagle";
        ammoCount = 15;
        fireRate = 0.5f;
        damage = 20f;
        aimTime = 1f;
    }

    // Override Shoot method if specific to this type of pistol
    public override void Shoot()
    {
        base.Shoot();
        // Additional logic for Pistol Type A
        Debug.Log("Deagle specific shooting logic executed.");
    }
}
