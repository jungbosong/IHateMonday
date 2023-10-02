using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : CharacterController
{
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CallKeyDown();
        }

        if (Input.GetMouseButton(0))
        {
            CallKeyPress();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CallKeyUp();
        }
    }
    public void OnInteraction()
    {
        CallInteractionEvent();
        Camera.main.GetComponent<ShakeCamera>().Shake(ShakeType.Attack);
    }
    public void OnMove(InputValue value)
    {
        // Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        if (newAim.magnitude >= .9f)
        {
            CallLookEvent(worldPos);
        }
    }

    public void OnFire(InputValue value)
    {
        IsAttacking = value.isPressed;
    }

    public void OnChangeActive()
    {
        CallChangeActiveEvent();
    }
    public void OnUseActive( )
    {
        CallUseActiveEvent();
    }
    public void OnThrowWeapon()
    {
        CallThrowWeaponEvent();
    }
    public void OnChangeWeapon()
    {
        Camera.main.GetComponent<ShakeCamera>().Shake(ShakeType.Hit);
        CallChangeWeaponEvent();
    }

    public void OnReload()
    {
        CallReloadEvent();
    }
}