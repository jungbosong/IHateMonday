using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurstGun : Gun
{
    [SerializeField] protected int _maxBurstBullet;
    [SerializeField] protected int _burstedBullet;
    [SerializeField] protected bool _isShooting;
    [SerializeField] protected float _shootingDelay;

    protected override void Awake()
    {
        base.Awake();
        _gunType = GunType.Burst;
    }
}

