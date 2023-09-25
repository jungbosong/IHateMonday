using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharzingGun : Gun
{
    [SerializeField] protected float _minShotCharzing;
    [SerializeField] protected float _maxShotCharzing;
    [SerializeField] protected float _shotCharzing;
    [SerializeField] protected bool _isShooting;
    [SerializeField] protected float _minCharzingValaue;
    [SerializeField] protected float _maxCharzingValaue;

    protected virtual void Awake()
    {
        _gunType = GunType.Charzing;
    }
}
