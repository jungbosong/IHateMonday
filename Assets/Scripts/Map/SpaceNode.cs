using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType
{
    Normal,
    NoneMonster,
    Wave,
    Box,
    Boss,
    Altar,
}

public class SpaceNode
{
    public SpaceNode leftSpace;     // 왼쪽 영역
    public SpaceNode rightSpace;    // 오른쪽 영역
    public SpaceNode parentSpace;   // 부모 영역
    public RectInt spaceRect;       // 분리된 영역의 Rect 정보
    public RectInt roomRect;        // 분리된 영역 내부의 방의 Rect정보
    public Vector2Int center
    {
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width/2, roomRect.y + roomRect.height/2);
        }
    }
    // TODO 각 룸에 타입 적용하기
    //public RoomType roomType;

    public SpaceNode(RectInt rect)
    {
        this.spaceRect = rect;
    }
}
