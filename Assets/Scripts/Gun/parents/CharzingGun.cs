using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharzingGun : Gun
{
    [Header ("CharzingGunStats")]
    [SerializeField] protected float _minShotCharzing;
    [SerializeField] protected float _maxShotCharzing;
    [SerializeField] protected float _minCharzingValaue;
    [SerializeField] protected float _maxCharzingValaue;

    protected float _shotCharzing;
    protected bool _isShooting;

    protected override void Awake()
    {
        base.Awake();
        _gunType = GunType.Charzing;
    }
}
