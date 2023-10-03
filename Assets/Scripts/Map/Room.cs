using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.Collections.AllocatorManager;

public enum RoomType
{
    Normal,         // 기본방
    NoneMonster,    // 몬스터가 없는 방
    Wave,           // 웨이브가 있는 방
    Box,            // 상자가 있는 방
    Boss,           // 보스 방
    Altar,          // 제단 방
}

public class Room : MonoBehaviour
{
    public Vector3 center;      // 방의 중심 좌표
    public float width;         // 방의 넓이    
    public float height;        // 방의 높이
    public RoomType type;       // 방 종류
    public bool firstEntered;

    public Action OnBattleStart;
    public Action OnBattleEnd;

    private void Start()
    {
        if(type == RoomType.Wave || type == RoomType.Boss)
        {
            Debug.Log(this.name);
            center = this.transform.GetChild(2).transform.position;
        }
        firstEntered = true;
    }
    public Room(Vector3 center, float width, float height, RoomType type)
    {
        this.center = center;
        this.width = width;
        this.height = height;
        this.type = type;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == RoomType.Wave || type == RoomType.Boss)
        {
            if(firstEntered)
            {
                Debug.Log($"first started {this.name}");
                Managers.Game.StartWave(this.transform.GetChild(2).transform.position, this);
                this.OnBattleStart?.Invoke();
                firstEntered = false;
            }
        }
    }
}