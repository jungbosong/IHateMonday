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
    public event Action OnChangeActiveEven;
    public event Action OnUseActiveEven;
    public event Action OnThrowWeaponEven;
    public event Action OnChangeWeaponEven;


    protected bool IsAttacking { get; set; }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }
    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }
    /*public void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }*/
    public void CallChangeActiveEvent()
    {
        OnChangeActiveEven?.Invoke();

    }
    public void CallUseActiveEvent()
    {
        OnUseActiveEven?.Invoke();

    }
    public void CallThrowWeaponEvent()
    {
        OnThrowWeaponEven?.Invoke();

    }
    public void CallChangeWeaponEvent()
    {
        OnChangeWeaponEven?.Invoke();

    }

}