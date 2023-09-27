using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaserGun : Gun
{
    protected bool _isShooting;

    protected override void Awake()
    {
        base.Awake();
        _gunType = GunType.Laser;
    }
}
