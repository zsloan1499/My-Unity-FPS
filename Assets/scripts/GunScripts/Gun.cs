using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Gun : MonoBehaviour
{
    public string gunName;
    
    public int ammoCount;

    public int magazineSize;

    public float fireRate;

    public float damage;

    public float headshotDamage;

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
