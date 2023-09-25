using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SemiAutoGun : Gun
{
    [Header ("SemiGunStats")]
    [SerializeField] protected float _manualFireDelay;

    [HideInInspector]public bool isManualFireReady = true;
    protected override void Awake()
    {
        base.Awake ();
        _gunType = GunType.SemiAuto;
    }
}
