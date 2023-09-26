using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO CharacterController 상속받도록 수정
// TODO GameManager 추가 및 역할 논의 -> Player를 GameManager가 들고 있게 할 것인지
public class EnemyController : MonoBehaviour
{
    //GameManager gamemanager;
    protected Transform ClosestTarget { get; private set; }

    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    protected virtual void Start()
    {
        //gameManager = GameManager.instance;
        //ClosestTarget = gameManager.Player;
    }

    protected virtual void FixedUpdate()
    {

    }

    // 현재 Enemy와 ClosestTarget 사이 거리를 구함
    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, ClosestTarget.position);
    }

    // 현재 Enemy가 ClosestTarget을 바라보는 방향 벡터를 구함
    protected Vector2 DirectionToTarget()
    {
        return (ClosestTarget.position - transform.position).normalized;
    }
}
