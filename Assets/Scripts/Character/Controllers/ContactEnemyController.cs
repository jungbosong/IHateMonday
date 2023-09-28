using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Player 코드 완성되면 HealthSystem 구조 완성해서 주석 풀기
public class ContactEnemyController : EnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool _isCollidingWithTarget;

    [SerializeField] private SpriteRenderer _characterSpriteRenderer;

    // private HealthSystem _healthSystem;
    // private HealthSystem _collidingTargetHealthSystem;
    // private CharacterMovement _collidingMovement;

    protected override void Start()
    {
        base.Start();
        // _healthSystem = GetComponent<HealthSystem>();
        // _healthSystem.OnDamage += Damage;
    }

    // 데미지를 받았을 때
    private void OnDamage()
    {
        // followRange를 크게 조정하여 target을 무조건 따라가도록 만듦
        followRange = 100f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // target과 부딪혔다면 (bool 변수는 OnTrigger 함수에서 설정)
        if (_isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {   
            // target이 범위 내에 들어오면 target을 따라감
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);    // direction 방향으로 움직이도록 MoveEvent 호출
        Rotate(direction);
    }

    // 부딪힌 대상의 체력을 변화시킴
    private void ApplyHealthChange()
    {
        // AttackSO attackSO = Stats.CurrentStats.attackSO;
        //_collidingTargetHealthSystem.ChangeHealth(-attackSO.power);
        //      // 넉백 효과가 있는 공격을 가지고 부딪힌 대상의 CharacterMovement 컴포넌트가 있을 경우(움직일 수 있는 대상인 경우)
        //if (attackSO.isOnKnockback && _collidingMovement != null)
        //{
        //      // 부딪힌 대상에 넉백 효과 적용
        //      _collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
        //}
    }

    // 움직이는 방향인 direction에 따라서 Enemy의 Sprite가 뒤집히도록
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _characterSpriteRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject receiver = collision.gameObject;

        // 부딪힌 대상이 target(Player)가 아니라면 return
        if (!receiver.CompareTag(targetTag)) return;

        // 부딪힌 대상의 HealthSystem을 가져옴
        // _collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        // 부딪힌 대상이 HealthSystem을 가질 경우(체력이 있는 오브젝트일 경우)
        // if (_collidingTargetHealthSystem != null) 
        // {
        //      // 대상과 부딪혔음을 표시
        //      _isCollidingWithTarget = true;
        // }

        // _collidingMovement = receiver.GetComponent<CharacterMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) return;

        // 대상과 부딪힘 표시를 해제
        // _isCollidingWithTarget = false;
    }
}