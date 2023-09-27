using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override
}

[SerializeField]
public class CharacterStats
{
    public StatsChangeType statsChangeType;
    public int MAX_HP = 8;  //최대 보유 체력
    public const int MAX_SHIELD_COUNT = 2; //최대 보유 쉴드
    public const float MAX_ATTACK_POWER = 200f;  //최대 공격력 증가 계수
    public const float MAX_ATTACK_SPEED = 200f;  //최대 공격속도 증가 계수
    public const float MAX_MOVE_SPEED = 200f;  //최대 이동속도 증가 계수
    
    public int currentHp = 5;  //현재 체력
    public int currentMaxHp = 5; //현재 최대 보유 체력
    public int shieldCount = 0;  // 현재 쉴드가능 횟수
    public float attackPowerCoefficient = 100f;  //현제 공격력 증가 계수
    public float attackSpeedCoefiicient = 100f; // 현재 공격속도 증가 계수
    public float moveSpeed = 5f;  //이동속도
    public float moveSpeedCoefficient = 100f; //현재 이동속도 증가 계수
    public bool isInvincible = false;  //무적 여부
    public float invincibilityTime = 2f;  //무적 지속 시간
}
