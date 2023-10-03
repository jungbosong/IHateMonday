using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    //public bool notDone = true;

    public Action OnBattleStart;
    public Action OnBattleEnd;
    public Room(Vector3 center, float width, float height, RoomType type)
    {
        this.center = center;
        this.width = width;
        this.height = height;
        this.type = type;
    }

    private void Awake()
    {
        center = this.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"now entered {type} room");
        Debug.Log($"center({this.center.x}, {this.center.y})");

        if(type == RoomType.NoneMonster)
        {
            return;
        }
        else
        {
            Managers.Game.StartWave(this);
            /*if (notDone)
            {
                Managers.Game.StartWave(this);
                notDone = false;
            }*/
            //OnBattleStart?.Invoke();
        }
    }
}