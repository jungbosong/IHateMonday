using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurstGun : Gun
{
    [Header ("BurstSetting")]
    [SerializeField] protected int _maxBurstBullet;
    protected int _burstedBullet = 0;
    [SerializeField] protected float _shootingDelay;


    protected override void Awake()
    {
        base.Awake();
        _gunType = GunType.Burst;
    }
}

