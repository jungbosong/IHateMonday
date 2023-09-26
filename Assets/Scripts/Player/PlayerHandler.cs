using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private const int MAX_HP = 8;  //최대 보유 체력
    private const int MAX_SHIELD_COUNT = 2; //최대 보유 쉴드
    private const float MAX_ATTACK_POWER = 200f;  //최대 공격력 증가 계수
    private const float MAX_ATTACK_SPEED = 200f;  //최대 공격속도 증가 계수
    private const float MAX_MOVE_SPEED = 200f;  //최대 이동속도 증가 계수

    
    private int currentHp = 5;  //현재 체력
    private int currentMaxHp = 5; //현재 최대 보유 체력
    private int shieldCount = 0;  // 현재 쉴드가능 횟수
    private float attackPowerCoefficient = 100f;  //현제 공격력 증가 계수
    private float attackSpeedCoefiicient = 100f; // 현재 공격속도 증가 계수
    private float moveSpeed = 5f;  //이동속도
    private float moveSpeedCoefficient = 100f; //현재 이동속도 증가 계수
    private bool isInvincible = false;  //무적 여부
    private float invincibilityTime = 2f;  //무적 지속 시간


    private void Start()
    {
    }
    public int GetCurrentHp()
    {
        return currentHp;
    }
    public void SetCurrentHp(int change)
    {
        
    }

    public int GetCurrentMaxHp()
    {
        return currentMaxHp;
    }
    public void SetCurrentMaxHp()
    {

    }

    public int GetShieldCount()
    {
        return shieldCount;
    }
    public void SetShieldCount()
    {

    }

    public float GetAttackPowerCoefficient()
    {
        return attackPowerCoefficient;
    }
    public void SetAttackPowerCoefficient()
    {

    }
    public float GetAttackSpeedCoefiicient()
    {
        return attackSpeedCoefiicient;
    }
    public void SetAttackSpeedCoefiicient()
    {

    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void SetMoveSpeed()
    {

    }

    public float GetMoveSpeedCoefficient()
    {
        return moveSpeedCoefficient;
    }
    public void SetMoveSpeedCoefficient()
    {

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
