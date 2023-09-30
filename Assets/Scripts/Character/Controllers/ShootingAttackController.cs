using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterAttackData
{
    [Header("Attack Info")]
    public float speed;         // 총알 발사 속도
    public float duration;      // 총알 지속 시간
    public int power;         // 공격 데미지
    public LayerMask target;    // 공격 대상
    public GameObject projectilePrefab;

    [Header("Knock Back Info")]
    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackTime;

    public float spread;                    // 탄 퍼짐
    public int numberOfProjectilesPerShot;  // 한 번에 쏘는 총알 수
    public float multipleProjectilesAngle;  // 총알의 각도
}

public class ShootingAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask _levelCollisionLayer;

    [SerializeField] private MonsterAttackData _attackData;
    private float _currentDuration;     // 실행 시간
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!_isReady)
        {
            return;
        }

        _currentDuration += Time.deltaTime;  // 실제 시간 누적

        if (_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position);
        }

        // 사라지지 않았다면 이동
        _rigidbody.velocity = _direction * _attackData.speed;   // 투사체 속도
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_levelCollisionLayer.value == (_levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            // 벽에 부딪히고 조금 안쪽으로 들어오도록
            DestroyProjectile(collision.ClosestPoint(transform.position) - _direction * .2f);
        }
        else if (_attackData.target.value == (_attackData.target.value | (1 << collision.gameObject.layer)))
        {
            // 원거리 공격이 충돌했을 때
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ChangeHealth(-_attackData.power);
                if (_attackData.isOnKnockback)
                {
                    CharacterMovement movement = collision.GetComponent<CharacterMovement>();
                    if (movement != null)
                    {
                        movement.ApplyKnockback(transform, _attackData.knockbackPower, _attackData.knockbackTime);
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position));
        }
    }

    public void InitializeAttack(Vector2 direction)
    {
        _direction = direction;

        _trailRenderer.Clear();
        _currentDuration = 0;

        transform.right = _direction;
        _isReady = true;
    }

    private void DestroyProjectile(Vector3 position)
    {
        gameObject.SetActive(false);
    }
}
