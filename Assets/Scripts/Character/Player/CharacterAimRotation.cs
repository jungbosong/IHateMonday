using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer;
    [SerializeField] private Transform armPovot;
    [SerializeField] private SpriteRenderer characterRenderer;
    private CharacterController _controller;
    // Start is called before the first frame update

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
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        armRenderer.flipY = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = armRenderer.flipY;
        armPovot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}