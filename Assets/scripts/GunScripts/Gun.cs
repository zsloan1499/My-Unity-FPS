using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Gun : MonoBehaviour
{
    public string gunName = "Glock";
    public float fireRate = 0.7f;
    public float damage = 25f;
    public int ammoCount = 50;
    public int maxAmmoCount = 150;
    public float headshotDamage = 50f;
    public int magazineSize = 10;


    public virtual void Shoot()
    {
        // Implement shooting logic common to all guns
        Debug.Log(gunName + " fired!");
    }

    public virtual void Reload()
    {
        // Implement reloading logic common to all guns
        Debug.Log(gunName + " reloaded!");
    }
}
