using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SemiAutoGun : Gun
{
    [SerializeField] protected float _manualFireDelay;

    protected virtual void Awake()
    {
        _gunType = GunType.SemiAuto;
    }
}
