using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserGun : Gun
{
    [SerializeField] protected float _isShooting;

    protected virtual void Awake()
    {
        _gunType = GunType.Laser;
    }
}
