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
    protected Animator _animator;

    protected GameObject _user;
    protected bool _isMonsterWeapon;
    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out _animator);
         _magazine = _maxMagazine;
        _ammunition = _maxAmmunition;
        _camera = Camera.main;
        _isMonsterWeapon = false;
    }
    public GunType GetGunType()
    {
        return _gunType;
    }


    public void OnLook(Vector2 worldPos)
    {
        //유저에는 왼손좌표랑 오른손좌표도 필요하다.
        Vector2 userPosition = _user.transform.position;
        Vector2 userLeftHandPosition = _user.transform.GetChild(0).position;
        Vector2 userRightHandPosition = _user.transform.GetChild(1).position;

        Vector2 leftdir;
        Vector2 rightdir;

        if (_isMonsterWeapon)
        {
            worldPos = GameObject.FindWithTag("Player").transform.position;
        }

        if (( userPosition - worldPos ).magnitude < ( userPosition - userRightHandPosition ).magnitude)
            return;


        leftdir = ( worldPos - (Vector2)userLeftHandPosition ).normalized;
        rightdir = ( worldPos - (Vector2)userRightHandPosition ).normalized;

        float leftRotZ = Mathf.Atan2(leftdir.y , leftdir.x) * Mathf.Rad2Deg;
        float rightRotZ = Mathf.Atan2(rightdir.y , rightdir.x) * Mathf.Rad2Deg;

        if (isLeftHand && Mathf.Abs(leftRotZ) < 75 && Mathf.Abs(rightRotZ) < 105)
        {
            isLeftHand = false;
        }
        else if (!isLeftHand && Mathf.Abs(rightRotZ) > 105 && Mathf.Abs(leftRotZ) > 75)
        {
            isLeftHand = true;
        }

        float rotY = 0f;
        float rotZ = 0f;
        if (isLeftHand)
        {
            transform.position = userLeftHandPosition;
            rotZ = 180f - Mathf.Atan2(leftdir.y , leftdir.x) * Mathf.Rad2Deg;
            rotY = 180f;
        }
        else
        {
            transform.position = userRightHandPosition;
            rotZ = Mathf.Atan2(rightdir.y , rightdir.x) * Mathf.Rad2Deg;
            rotY = 0;
        }

        transform.rotation = Quaternion.Euler(0 , rotY , rotZ);
    }

    //장착 / 해제 시 플레이어쪽에서 불러야할 함수
    public void Equip(GameObject user , Action onKeyDown, Action onKeyPress, Action onKeyUp, Action onRoll, Action<Vector2> onLook)
    {
        _user = user;
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

        if(_animator != null)
        {
            _animator.SetBool("Reload" , false);
        }
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void MonsterEquip(GameObject user)
    {
        _user = user;
        _isMonsterWeapon = true;
    }

    public bool MonsterRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position , transform.right , 20 , LayerMask.GetMask("Wall") | LayerMask.GetMask("Player"));

        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
