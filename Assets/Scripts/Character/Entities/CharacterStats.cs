using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatsChangeType
{
    Add,        // 더하기
    Multiple,   // 곱하기
    Override,   // 덮어쓰기
}

[Serializable]
public class CharacterStats
{    
    public StatsChangeType statsChangeType;         // 스탯 타입
    public int currentHp = 5;                       // 현재 체력
    public int currentMaxHp = 5;                    // 현재 최대 보유 체력
    public int shieldCount = 0;                     //  현재 쉴드가능 횟수
    public float attackPowerCoefficient = 100f;     // 현제 공격력 증가 계수
    public float attackSpeedCoefiicient = 100f;     //  현재 공격속도 증가 계수
    [Range(1f, 20f)] public float moveSpeed = 5f;                    // 이동속도
    public float moveSpeedCoefficient = 100f;       // 현재 이동속도 증가 계수
    public bool isInvincible = false;               // 무적 여부
    public float invincibilityTime = 2f;            // 무적 지속 시간

    //public int GetCurrentHp()
    //{
    //    return currentHp;
    //}
    //public void SetCurrentHp(int change)
    //{
    //    currentHp += change;
    //}

    //public int GetCurrentMaxHp()
    //{
    //    return currentMaxHp;
    //}
    //public void SetCurrentMaxHp(int change)
    //{
    //    currentMaxHp += change;
    //}

    //public int GetShieldCount()
    //{
    //    return shieldCount;
    //}
    //public void SetShieldCount(int change)
    //{
    //    shieldCount += change;
    //}

    //public float GetAttackPowerCoefficient()
    //{
    //    return attackPowerCoefficient;
    //}
    //public void SetAttackPowerCoefficient(float change)
    //{
    //    attackPowerCoefficient += change;
    //}
    //public float GetAttackSpeedCoefiicient()
    //{
    //    return attackSpeedCoefiicient;
    //}
    //public void SetAttackSpeedCoefiicient(float change)
    //{
    //    attackPowerCoefficient += change;
    //}

    //public float GetMoveSpeed()
    //{
    //    return moveSpeed;
    //}
    //public void SetMoveSpeed(float change)
    //{
    //    moveSpeed += change;
    //}

    //public float GetMoveSpeedCoefficient()
    //{
    //    return moveSpeedCoefficient;
    //}
    //public void SetMoveSpeedCoefficient(float change)
    //{
    //    moveSpeedCoefficient += change;
    //}

    //public bool GetIsInvincible()
    //{
    //    return isInvincible;
    //}
    //public void SetIsInvincible()
    //{

    //}

    //public float GetInvincibilityTime()
    //{
    //    return invincibilityTime;
    //}
    //public void SetInvincibilityTime()
    //{

    //}
}
