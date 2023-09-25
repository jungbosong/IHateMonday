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

    public abstract IEnumerator COReload(float time);
    public abstract IEnumerator COFire(float time);


    protected GunType _gunType;

    protected Transform _shotPoint;

    [SerializeField] protected int _magazine;
    [SerializeField] protected int _maxMagazine;
    [SerializeField] protected int _ammunition;
    [SerializeField] protected int _maxAmmunition;
    [SerializeField] protected float _reloadDelay;
    [SerializeField] protected float _autoFireDelay;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _bulletDistance;
    [SerializeField] protected float _knockBack;
    [SerializeField] protected float _accuracy;


    public bool isEquip;
    public bool isShootAble;


    public GunType GetGunType()
    {
        return _gunType;
    }
}
