using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemyController : EnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        //IsAttacking = false;
        // target이 범위 내에 있을 때 따라가기
        if (distance <= followRange)
        {
            // target이 사격 거리 내에 있을 때
            if (distance <= shootRange)
            {
                //int layerMaskTarget = Stats.CurrentStats.attackSO.target;

                //RaycastHit2D hit = Physics.Raycast(transform.position, direction, 11f,
                //    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                //if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                //{
                //    CallLookEvent(direction);       // 바라보는 방향 바꾸기
                //    CallMoveEvent(Vector2.zero);    // 총을 쏠 때는 움직이지 않음
                //    IsAttacking = true;
                //}
                //else
                //{
                //    CallMoveEvent(direction);       // 사격 거리 내에 target이 있지 않을 때 target을 향해 움직임
                //    Rotate(direction);
                //}
            }
            else
            {
                CallMoveEvent(direction); // target이 범위 내에 있지 않을 때 target을 향해 움직임
                Rotate(direction);
            }
        }
    }
}
