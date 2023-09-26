using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerHandler _playerHandler;
    private Vector2 _movemetDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private Vector2 _knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _playerHandler = GetComponent<PlayerHandler>();
        _controller.OnMoveEvent += Move;
    }
    private void FixedUpdate()
    {
        ApplyMovement(_movemetDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;  
        }
    }

    private void Move(Vector2 direction)
    {
        _movemetDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        _knockback = -(other.position - transform.position).normalized * power;
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * _playerHandler.GetMoveSpeed();
        if (knockbackDuration > 0.0f)
        {
            direction += _knockback;
        }
        _rigidbody.velocity = direction;
    }
}