using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun : WeaponScript
{
    public bool Equiped;
    protected int Bullets;
    protected GameObject ps;

    private void Start()
    {
        ps = Resources.Load<GameObject>("Prefabs/Particle/GunPs");
    }
    public abstract void Shot();
    protected abstract void Drop();
    protected abstract void Pickup();
}
