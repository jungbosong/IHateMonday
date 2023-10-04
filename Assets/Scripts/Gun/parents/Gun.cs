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
    [SerializeField] protected int _bulletCount = 1;

    [HideInInspector]public bool isEquip;
    [HideInInspector]public bool isAutoFireReady = true;
    [HideInInspector]public bool isReload = false;
    protected bool isLeftHand = true;
    protected Animator _animator;
    protected bool isGuied = false;
    protected int buffBullet = 10;

    protected GameObject _player;
    protected PlayerStatsHandler _stat;
    public int GetMagazine() { return _magazine; } //현재 탄창에 장착되어있는 총알수
    public int GetMaxMagazine() { return _maxMagazine; } //최대탄창
    public int GetAmmunition() { return _ammunition; } //현재 가진 탄피 갯수
    public int GetMaxAmmunition() { return _maxAmmunition; } // 최대 탄피 갯수

    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out _animator);
         _magazine = _maxMagazine;
        _ammunition = _maxAmmunition;
        _camera = Camera.main;

        _player = GameObject.FindWithTag("Player");
        _stat = _player.GetComponent<PlayerStatsHandler>();


    }
    public GunType GetGunType()
    {
        return _gunType;
    }

    protected virtual void Update()
    {
        if(isLeftHand)
        {
            transform.position = _player.transform.GetChild(0).position;
        }
        else
        {
            transform.position = _player.transform.GetChild(1).position;
        }
    }
    public void OnLook(Vector2 worldPos)
    {
        //�������� �޼���ǥ�� ��������ǥ�� �ʿ��ϴ�.
        Vector2 userPosition = _player.transform.position;
        Vector2 userLeftHandPosition = _player.transform.GetChild(0).position;
        Vector2 userRightHandPosition = _player.transform.GetChild(1).position;

        Vector2 leftdir;
        Vector2 rightdir;

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
            rotZ = 180f - Mathf.Atan2(leftdir.y , leftdir.x) * Mathf.Rad2Deg;
            rotY = 180f;
        }
        else
        {
            rotZ = Mathf.Atan2(rightdir.y , rightdir.x) * Mathf.Rad2Deg;
            rotY = 0;
        }

        transform.rotation = Quaternion.Euler(0 , rotY , rotZ);
    }

    //���� / ���� �� �÷��̾��ʿ��� �ҷ����� �Լ�
    public void Equip(ref Action onKeyDown, ref Action onKeyPress , ref Action onKeyUp, ref Action onRoll, ref Action<Vector2> onLook, ref Action onReload)
    {
        isEquip = true;
        isAutoFireReady = true;
        isReload = false;

        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;
        onLook -= OnLook;
        onReload -= OnReload;
        onKeyDown += OnKeyDown;
        onKeyPress += OnKeyPress;
        onKeyUp += OnKeyUp;
        onRoll += OnRoll;
        onLook += OnLook;
        onReload += OnReload;

        gameObject.SetActive(true);

        
        OnLook(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    public void UnEquip(ref Action onKeyDown , ref Action onKeyPress, ref Action onKeyUp, ref Action onRoll, ref Action<Vector2> onLook, ref Action onReload)
    {
        isEquip = false;
        onKeyDown -= OnKeyDown;
        onKeyPress -= OnKeyPress;
        onKeyUp -= OnKeyUp;
        onRoll -= OnRoll;
        onLook -= OnLook;
        onReload -= OnReload;

        StopAllCoroutines();
        gameObject.SetActive(false);
        if (_animator != null)
        {
            _animator.SetBool("Reload" , false);
        }
    }

    public void OnReload()
    {
        if (_magazine == _maxMagazine)
            return;
        if (_ammunition == 0)
            return;
        if (_ammunition == _magazine)
            return;

        StartCoroutine(COReload());
    }

    public void AddAmmunition(int amount)   //�нú� ������ ź�� ����
    {
        _ammunition += amount;
    }

    public void ChargingAmmunition()    //���� ��� �� ź�� ����
    {
        _maxAmmunition = (int)(_maxAmmunition * 1.25f);
        _ammunition = _maxAmmunition;
    }
    
    public void Lostmmunition(float random)     //���� ��� �� ź�� ����
    {
        _ammunition = _ammunition - (int)(_ammunition * random);
    }

    protected float GetDamage(float baseDamage)
    {
        return baseDamage * _stat.CurrentStats.attackPowerCoefficient / 100f;
    }
    protected float GetSpeed(float baseDelay)
    {
        return baseDelay * _stat.CurrentStats.attackSpeedCoefiicient / 100f;
    }
}
