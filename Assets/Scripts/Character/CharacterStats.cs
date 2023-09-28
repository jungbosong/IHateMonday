using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//TODO
//제한된 값을 넘지 않도록 set함수 조건 조정

[Serializable]
public class CharacterStats
{
    [SerializeField] private int MAX_HP = 8;  //최대 보유 체력
    [SerializeField] private const int MAX_SHIELD_COUNT = 2; //최대 보유 쉴드
    [SerializeField] private const float MAX_ATTACK_POWER = 200f;  //최대 공격력 증가 계수
    [SerializeField] private const float MAX_ATTACK_SPEED = 200f;  //최대 공격속도 증가 계수
    [SerializeField] private const float MAX_MOVE_SPEED = 200f;  //최대 이동속도 증가 계수
    
    [SerializeField] private int currentHp = 5;  //현재 체력
    [SerializeField] private int currentMaxHp = 5; //현재 최대 보유 체력
    [SerializeField] private int shieldCount = 0;  // 현재 쉴드가능 횟수
    [SerializeField] private float attackPowerCoefficient = 100f;  //현제 공격력 증가 계수
    [SerializeField] private float attackSpeedCoefiicient = 100f; // 현재 공격속도 증가 계수
    [SerializeField] private float moveSpeed = 5f;  //이동속도
    [SerializeField] private float moveSpeedCoefficient = 100f; //현재 이동속도 증가 계수
    [SerializeField] private bool isInvincible = false;  //무적 여부
    [SerializeField] private float invincibilityTime = 2f;  //무적 지속 시간

    public int GetCurrentHp()
    {
        return currentHp;
    }
    public void SetCurrentHp(int change)
    {
        currentHp += change;
    }

    public int GetCurrentMaxHp()
    {
        return currentMaxHp;
    }
    public void SetCurrentMaxHp(int change)
    {
        currentMaxHp += change;
    }

    public int GetShieldCount()
    {
        return shieldCount;
    }
    public void SetShieldCount(int change)
    {
        shieldCount += change;
    }

    public float GetAttackPowerCoefficient()
    {
        return attackPowerCoefficient;
    }
    public void SetAttackPowerCoefficient(float change)
    {
        attackPowerCoefficient += change;
    }
    public float GetAttackSpeedCoefiicient()
    {
        return attackSpeedCoefiicient;
    }
    public void SetAttackSpeedCoefiicient(float change)
    {
        attackPowerCoefficient += change;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void SetMoveSpeed(float change)
    {
        moveSpeed += change;
    }

    public float GetMoveSpeedCoefficient()
    {
        return moveSpeedCoefficient;
    }
    public void SetMoveSpeedCoefficient(float change)
    {
        moveSpeedCoefficient += change;
    }

    public bool GetIsInvincible()
    {
        return isInvincible;
    }
    public void SetIsInvincible()
    {

    }

    public float GetInvincibilityTime()
    {
        return invincibilityTime;
    }
    public void SetInvincibilityTime()
    {

    }
}
