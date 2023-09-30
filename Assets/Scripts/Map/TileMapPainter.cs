using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapPainter : MonoBehaviour
{
    #region Tiles
    [SerializeField] private Tilemap _tileMap;      // 타일이 그려지는 곳
    [SerializeField] private Tile roomTile;         // 방을 표현할 타일
    [SerializeField] private Tile roadTile;         // 길을 표현할 타일
    [SerializeField] private Tile wallTile;         // 벽을 표현할 타일
    [SerializeField] private Tile outTile;          // 외곽지역(빈공간)을 표현할 타일
    #endregion
    
    //배경을 채우는 함수
    public void FillBackground(Vector2Int mapSize)
    {
        for (int i = -10; i < mapSize.x + 10; i++)
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }

    //룸 타일과 바깥 타일이 만나는 부분
    public void FillWall(Vector2Int mapSize)
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (_tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //바깥타일 일 경우
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;
                            if (_tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //방의 Rect정보를 받아서 tile을 설정하는 함수
    public void FillRoom(Vector2Int mapSize, Rect rect)
    {
        int x = (int)System.Math.Round(rect.x);
        int y = (int)System.Math.Round(rect.y);
        int width = (int)System.Math.Round(rect.width);
        int height = (int)System.Math.Round(rect.height);

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }

    public void FillRoad(Vector3 leftNodeCenter, Vector3 rightNodeCenter, Vector2Int mapSize)
    {
        int startX = (int)System.Math.Round(System.Math.Min(leftNodeCenter.x, rightNodeCenter.x));
        int startY = (int)System.Math.Round(System.Math.Min(leftNodeCenter.y, rightNodeCenter.y));

        int endX = (int)System.Math.Round(System.Math.Max(leftNodeCenter.x, rightNodeCenter.x));
        int endY = (int)System.Math.Round(System.Math.Max(leftNodeCenter.y, rightNodeCenter.y));

        for (int i = startX; i <= endX; i++)
        {
            _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (int)System.Math.Round(leftNodeCenter.y) - mapSize.y / 2 - 1, 0), roadTile);
            _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (int)System.Math.Round(leftNodeCenter.y) - mapSize.y / 2, 0), roadTile);
            _tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (int)System.Math.Round(leftNodeCenter.y) - mapSize.y / 2 + 1, 0), roadTile);
        }

        for (int j = startY; j <= endY; j++)
        {
            _tileMap.SetTile(new Vector3Int((int)System.Math.Round(rightNodeCenter.x) - mapSize.x / 2 - 1, j - mapSize.y / 2, 0), roadTile);
            _tileMap.SetTile(new Vector3Int((int)System.Math.Round(rightNodeCenter.x) - mapSize.x / 2, j - mapSize.y / 2, 0), roadTile);
            _tileMap.SetTile(new Vector3Int((int)System.Math.Round(rightNodeCenter.x) - mapSize.x / 2 + 1, j - mapSize.y / 2, 0), roadTile);
        }
    }
}