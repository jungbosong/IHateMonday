using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootingAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask _levelCollisionLayer;
    
    private float _currentDuration;     // 실행 시간
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;

    private MonsterAttackDataSO _monsterAttackDataSO;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isReady)
        {
            return;
        }

        _currentDuration += Time.deltaTime;  // 실제 시간 누적

        if (_currentDuration > _monsterAttackDataSO.duration)
        {
            Destroy(gameObject);
        }

        // 사라지지 않았다면 이동
        _rigidbody.velocity = _direction * _monsterAttackDataSO.speed;   // 투사체 속도
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_levelCollisionLayer.value == (_levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            // 벽에 부딪히고 조금 안쪽으로 들어오도록
            Destroy(gameObject);
        }
        else if (_monsterAttackDataSO.target.value == (_monsterAttackDataSO.target.value | (1 << collision.gameObject.layer)))
        {
            // 원거리 공격이 충돌했을 때
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ChangeHealth(-_monsterAttackDataSO.power);
                if (_monsterAttackDataSO.isOnKnockback)
                {
                    CharacterMovement movement = collision.GetComponent<CharacterMovement>();
                    if (movement != null)
                    {
                        movement.ApplyKnockback(transform, _monsterAttackDataSO.knockbackPower, _monsterAttackDataSO.knockbackTime);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    public void InitializeAttack(Vector2 direction, MonsterAttackDataSO monsterAttackDataSO)
    {
        _monsterAttackDataSO = monsterAttackDataSO;
        _direction = direction;

        _currentDuration = 0;

        transform.right = _direction;
        _isReady = true;
    }
}
