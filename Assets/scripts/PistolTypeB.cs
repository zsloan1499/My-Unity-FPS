using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolTypeB : Pistol
{
    public PistolTypeB()
    {
        gunName = "Pistol Type B";
        ammoCount = 10;
        fireRate = 0.7f;
        damage = 25f;
        aimTime = 0.8f;
    }

    // Override Shoot method if specific to this type of pistol
    public override void Shoot()
    {
        base.Shoot();
        // Additional logic for Pistol Type B needs to be added later
        Debug.Log("Pistol Type B specific shooting logic executed.");
    }
}