using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public abstract void OnLook(Vector2 worldPos);

    public abstract IEnumerator COReload();
    public abstract IEnumerator COFire();


    protected GunType _gunType;

    [SerializeField]protected Transform _shotPoint;
    protected Camera _camera;

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
    protected bool isLeftHand = true;

    protected virtual void Awake()
    {
       
        _magazine = _maxMagazine;
        _ammunition = _maxAmmunition;
        _camera = Camera.main;
    }
    public GunType GetGunType()
    {
        return _gunType;
    }

    //장착 / 해제 시 플레이어쪽에서 불러야할 함수
    public void Equip(Action onKeyDown, Action onKeyPress, Action onKeyUp, Action onRoll, Action<Vector2> onLook)
    {
        isEquip = true;
        isAutoFireReady = true;
        isReload = false;

        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;
        onLook -= OnLook;
        onKeyDown += OnKeyDown;
        onKeyPress += OnKeyPress;
        onKeyUp += OnKeyUp;
        onRoll += OnRoll;
        onLook -= OnLook;

        gameObject.SetActive(true);
    }
    public void UnEquip(Action onKeyDown, Action onKeyPress, Action onKeyUp, Action onRoll, Action<Vector2> onLook)
    {
        isEquip = false;
        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;
        onLook -= OnLook;

        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
