using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GunTemplate
{
    public int ammo;
    public float damagePerShot = 25f;
    public float projectileSpeed = 2f;
    public float fireRate = 1f;
    public int bulletsPerShot = 1, bulletsLeft;
    public float spread;
    public float zoomFOV = 60f;
}
