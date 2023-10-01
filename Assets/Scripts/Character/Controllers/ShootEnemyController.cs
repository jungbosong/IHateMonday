using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : EnemyController
{
    [SerializeField] private float _followRange = 15f;
    [SerializeField] private float _shootRange = 10f;

    // 마지막으로 공격한 시간
    private float _timeSinceLastAttack = float.MaxValue;

    public MonsterAttackDataSO monsterAttackDataSO;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        //IsAttacking = false;
        // target이 범위 내에 있을 때 따라가기
        if (distance <= _followRange)
        {
            // target이 사격 거리 내에 있을 때
            if (distance <= _shootRange)
            {
                int layerMaskTarget = monsterAttackDataSO.target;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, 
                    (1 << LayerMask.NameToLayer("Wall")) | layerMaskTarget);

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    CallLookEvent(direction);       // 바라보는 방향 바꾸기
                    CallMoveEvent(Vector2.zero);    // 총을 쏠 때는 움직이지 않음
                    IsAttacking = true;
                    HandleAttackDelay();
                }
                else
                {
                    CallMoveEvent(direction);       // 사격 거리 내에 target이 있지 않을 때 target을 향해 움직임
                    Rotate(direction);
                }
            }
            else
            {
                CallMoveEvent(direction); // target이 범위 내에 있지 않을 때 target을 향해 움직임
                Rotate(direction);
            }
        }
    }

    private void HandleAttackDelay()
    {
        if (_timeSinceLastAttack <= monsterAttackDataSO.attackDelay)
        {
            _timeSinceLastAttack += Time.fixedDeltaTime;
        }

        if (IsAttacking && _timeSinceLastAttack > monsterAttackDataSO.attackDelay)
        {
            _timeSinceLastAttack = 0;
            CallAttackEvent();
        }
    }
}
