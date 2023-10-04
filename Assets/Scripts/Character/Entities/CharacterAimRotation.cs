using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPovot;
    [SerializeField] private SpriteRenderer characterRenderer;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    public void OnAim(Vector2 newAimDirection)
    {
        RotateArm(newAimDirection);
    }
    private void RotateArm(Vector2 direction)
    {
        Vector2 newAim = ( direction - (Vector2)transform.position ).normalized;

        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;
        //armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        //armPovot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}