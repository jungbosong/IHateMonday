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

    private Camera _mainCamera;
    private int currentHp = 5;
    private int currentMaxHp = 5;
    private int shieldCount = 0;
    private float attackPoweCoefficientr = 100f;
    private float attackSpeedCoefiicient = 100f;
    private float moveSpeed = 5f;
    private float moveSpeedCoefficient = 100f;
    private bool isInvincible = false;
    private float invincibilityTime;


    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
