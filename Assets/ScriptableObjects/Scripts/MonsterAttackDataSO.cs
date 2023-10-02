using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterAttackDataSO", order = 0)]
public class MonsterAttackDataSO : ScriptableObject
{
    [Header("Attack Info")]
    public float speed;         // 총알 발사 속도
    public float duration;      // 총알 지속 시간
    public int power;           // 공격 데미지
    public float attackDelay;   // 공격 딜레이
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
