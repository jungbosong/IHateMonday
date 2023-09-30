using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsChangeType
{
    Add,        // 더하기
    Multiple,   // 곱하기
    Override,   // 덮어쓰기
}

[Serializable]
public class PlayerStats: CharacterStats
{
    public StatsChangeType statsChangeType;         // 스탯 타입
    public int shieldCount = 0;                     //  현재 쉴드가능 횟수
    public float attackPowerCoefficient = 100f;     // 현제 공격력 증가 계수
    public float attackSpeedCoefiicient = 100f;     //  현재 공격속도 증가 계수
    public float moveSpeedCoefficient = 100f;       // 현재 이동속도 증가 계수
    public bool isInvincible = false;               // 무적 여부
    public float invincibilityTime = 2f;            // 무적 지속 시간
}
