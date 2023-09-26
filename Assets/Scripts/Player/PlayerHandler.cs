using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private const int MAX_HP = 8;
    private const int MAX_SHIELD_COUNT = 2;
    private const float MAX_ATTACK_POWER = 200f;
    private const float MAX_ATTACK_SPEED = 200f;
    private const float MAX_MOVE_SPEED = 200f;

    
    private int currentHp = 5;
    private int currentMaxHp = 5;
    private int shieldCount = 0;
    private float attackPowerCoefficient = 100f;
    private float attackSpeedCoefiicient = 100f;
    private float moveSpeed = 5f;
    private float moveSpeedCoefficient = 100f;
    private bool isInvincible = false;
    private float invincibilityTime = 2f;


   
    public int GetCurrentHp()
    {
        return currentHp;
    }
    public void SetCurrentHp()
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
