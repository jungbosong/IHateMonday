using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Auto,
    SemiAuto,
    Burst,
    Charzing,
    Laser
}
public abstract class Gun : MonoBehaviour
{
    public abstract void OnKeyDown();
    public abstract void OnKeyUp();
    public abstract void OnKeyPress();
    public abstract void OnRoll();

    public abstract IEnumerator COReload();
    public abstract IEnumerator COFire();


    protected GunType _gunType;

    [SerializeField]protected Transform _shotPoint;

    [Header("CheckInInstpacter")]
    [SerializeField]protected int _magazine;
    [SerializeField]protected int _ammunition;

    [Header ("GunStats")]
    [SerializeField] protected int _maxMagazine;
    [SerializeField] protected int _maxAmmunition;
    [SerializeField] protected float _reloadDelay;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _bulletDistance;
    [SerializeField] protected float _knockBack;
    [SerializeField] protected float _accuracy;
    [SerializeField] protected float _autoFireDelay;

    
    [HideInInspector]public bool isEquip;
    [HideInInspector]public bool isAutoFireReady = true;
    [HideInInspector]public bool isReload = false;

    protected virtual void Awake()
    {
        _magazine = _maxMagazine;
        _ammunition = _maxAmmunition;
    }
    public GunType GetGunType()
    {
        return _gunType;
    }

    public void Equip(Action onKeyDown, Action onKeyPress, Action onKeyUp, Action onRoll)
    {
        isEquip = true;

        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;
        onKeyDown += OnKeyDown;
        onKeyPress += OnKeyPress;
        onKeyUp += OnKeyUp;
        onRoll += OnRoll;

        gameObject.SetActive(true);
    }
    public void UnEquip(Action onKeyDown, Action onKeyPress, Action onKeyUp, Action onRoll)
    {
        isEquip = false;
        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;

        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
