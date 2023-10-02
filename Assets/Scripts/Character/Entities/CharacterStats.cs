using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterStats
{    
    public int currentHp = 5;                       // 현재 체력
    public int currentMaxHp = 5;                    // 현재 최대 보유 체력
    [Range(1f, 20f)] public float moveSpeed = 5f;                    // 이동속도
}
