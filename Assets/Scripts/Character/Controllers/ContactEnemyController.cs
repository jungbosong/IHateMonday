using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemyController : EnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    private HealthSystem _healthSystem;
    private HealthSystem _collidingTargetHealthSystem;
    //private CharacterMovement _collidingMovement; // TODO 카메라 쉐이킹 구현되면 삭제

    protected override void Start()
    {
        base.Start();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDamage += OnDamage;
    }

    // Player에게 공격 받으면 Player를 따라가도록
    private void OnDamage()
    {
        followRange = 100f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {   
            // 범위 내에 있으면 Player를 따라가도록
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction); 
        Rotate(direction);
    }

    // Player에게 데미지를 줌
    private void ApplyHealthChange()
    {
        _collidingTargetHealthSystem.ChangeHealth(-1);
        //      // 넉백 효과가 있는 공격을 가지고 부딪힌 대상의 CharacterMovement 컴포넌트가 있을 경우(움직일 수 있는 대상인 경우)
        //if (attackSO.isOnKnockback && _collidingMovement != null)
        //{
        //      // 부딪힌 대상에 넉백 효과 적용
        //      _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        // 부딪힌 대상이 target(Player)가 아니라면 return
        if (!receiver.CompareTag(targetTag)) return;

        _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (_collidingTargetHealthSystem != null)
        {
            // 대상과 부딪혔음을 표시
            _isCollidingWithTarget = true;
        }

        //_collidingMovement = receiver.GetComponent<CharacterMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) return;

        // 대상과 부딪힘 표시를 해제
        _isCollidingWithTarget = false;
    }
}