using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceNode
{
    public SpaceNode leftSpace;     // 왼쪽 영역
    public SpaceNode rightSpace;    // 오른쪽 영역
    public SpaceNode parentSpace;   // 부모 영역
    public Rect spaceRect;       // 분리된 영역의 Rect 정보
    public Rect roomRect;        // 분리된 영역 내부의 방의 Rect정보
    public Vector3 center
    {
        get
        {
            return new Vector3(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
    }
    public SpaceNode(Rect rect)
    {
        this.spaceRect = rect;
    }
}