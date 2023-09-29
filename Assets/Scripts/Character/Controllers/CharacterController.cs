using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;  
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;
    public event Action OnChangeActiveEvent;
    public event Action OnUseActiveEvent;
    public event Action OnThrowWeaponEvent;
    public event Action OnChangeWeaponEvent;

    // 마지막으로 공격한 시간
    private float _timeSinceLastAttack = float.MaxValue;
    protected bool IsAttacking { get; set; }
    protected CharacterStatsHandler Stats { get; private set; }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if (_timeSinceLastAttack <= 0.2f) 
        {
            _timeSinceLastAttack += Time.deltaTime;   
        }

        if (IsAttacking && _timeSinceLastAttack > 0.2f)
        {
            _timeSinceLastAttack = 0;
            CallAttackEvent();
        }
    }

    protected virtual void Awake()
    {
        Stats = GetComponent<CharacterStatsHandler>();
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }

    public void CallChangeActiveEvent()
    {
        OnChangeActiveEvent?.Invoke();
    }

    public void CallUseActiveEvent()
    {
        OnUseActiveEvent?.Invoke();
    }
    public void CallThrowWeaponEvent()
    {
        OnThrowWeaponEvent?.Invoke();
    }
    public void CallChangeWeaponEvent()
    {
        OnChangeWeaponEvent?.Invoke();
    }
}