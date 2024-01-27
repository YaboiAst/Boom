using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GunTemplate
{
    public int totalAmmo;
    public int magTotalAmmo;
    public float damagePerShot = 25f;
    public float projectileSpeed = 2f;
    public float fireRate = 1f;
    public float burstRate = 1f;
    public float reloadTime = 1f;
    public int bulletsPerShot = 1; 
    public float spread;
    public float recoil;
    public float zoomFOV = 60f;
    public bool canHoldFire;

    [HideInInspector] public int bulletsLeft;
    [HideInInspector] public int magCurrentAmmo;
}
