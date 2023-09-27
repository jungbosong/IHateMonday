using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    #region GenerateMapSetting
    [SerializeField] private Vector2Int _mapSize;       // 제작할 맵 크기
    [SerializeField] private float _minDevideRate;      // 나뉘는 영역의 최소 비율
    [SerializeField] private float _maxDevideRate;      // 나뉘는 영역의 최대 비율
    private int _maxDepth;                              // 영역을 자세하게 나누는 정도(깊이)
    private int[] _roomCounts = new int[6];             // 해당 난이도에서의 6가지 종류 방의 개수
    private List<int> _notGeneratedRoom = new List<int>(); // 아직 만들어지지 않은 방
    #endregion

    #region lineRenderer
    [SerializeField] private GameObject _line;          // 각 영역의 경계선을 표시하기 위한 line renderer
    [SerializeField] private GameObject _map;           // root 영역을 표시하기 위한 line renderer
    [SerializeField] private GameObject _roomLine;      // 방을 표시하기 위한 line renderer
    #endregion

    #region Tiles
    [SerializeField] private Tilemap _tileMap;      // 타일이 그려지는 곳
    [SerializeField] private Tile roomTile;         // 방을 표현할 타일
    [SerializeField] private Tile wallTile;         // 벽을 표현할 타일
    [SerializeField] private Tile outTile;          // 외곽지역(빈공간)을 표현할 타일
    #endregion

    void Awake()
    {
        Managers.Map.roomList.Clear();
        for (int i = 0; i < _roomCounts.Length; i++)
        {
            _roomCounts[i] = Managers.Map.roomCounts[Managers.Map.currentFloor,i];
            _notGeneratedRoom.Add(i);
        }
        switch(Managers.Map.currentFloor)
        {
            case 0:
                _maxDepth = 3;
                break;
            case 1:
                _maxDepth = 4;
                break;
            case 2:
                _maxDepth = 4;
                break;
        }
    }

    void Start()
    {
        FillBackground();
        SpaceNode root = new SpaceNode(new Rect(0, 0, _mapSize.x, _mapSize.y));
        //DrawMap(0, 0);
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall();
        // TODO 확인 후 지우기
        for (int i = 0; i < Managers.Map.roomList.Count; i++)
        {
            Room room = Managers.Map.roomList[i];
            Debug.Log($"type: {room.type}\ncenter: {room.center}\nwidth: {room.width}\nheight {room.height}");
        }
    }

    #region GenerateMap
    // 방을 n만큼의 깊이로 이진 분할
    private void Divide(SpaceNode tree, int n)
    {
        if (n == _maxDepth) return;

        float maxLength = Mathf.Max(tree.spaceRect.width, tree.spaceRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength * _minDevideRate, maxLength *_maxDevideRate));
        if(tree.spaceRect.width >= tree .spaceRect.height)
        {
            tree.leftSpace = new SpaceNode(new Rect(tree.spaceRect.x, tree.spaceRect.y, split, tree.spaceRect.height));
            tree.rightSpace = new SpaceNode(new Rect(tree.spaceRect.x + split, tree.spaceRect.y, tree.spaceRect.width - split, tree.spaceRect.height));
            //DrawLine(new Vector2(tree.spaceRect.x + split, tree.spaceRect.y), new Vector2(tree.spaceRect.x + split, tree.spaceRect.y + tree.spaceRect.height));
        }
        else
        {
            tree.leftSpace = new SpaceNode(new Rect(tree.spaceRect.x, tree.spaceRect.y, tree.spaceRect.width, split));
            tree.rightSpace = new SpaceNode(new Rect(tree.spaceRect.x, tree.spaceRect.y + split, tree.spaceRect.width, tree.spaceRect.height - split));
            //DrawLine(new Vector2(tree.spaceRect.x , tree.spaceRect.y+ split), new Vector2(tree.spaceRect.x + tree.spaceRect.width, tree.spaceRect.y  + split));
        }
        tree.leftSpace.parentSpace = tree;
        tree.rightSpace.parentSpace = tree;
        Divide(tree.leftSpace, n + 1);
        Divide(tree.rightSpace, n + 1);
    }

    // 리프영역에 해당 영역 보다 작은 사이즈의 방 제작
    private Rect GenerateRoom(SpaceNode tree, int n)
    {
        Rect rect;
        if (n == _maxDepth)
        {
            rect = tree.spaceRect;
            float width = Random.Range(rect.width / 2, rect.width - 1);
            float height = Random.Range(rect.height / 2, rect.height - 1);

            float x = rect.x + Random.Range(1, rect.width - width);
            float y = rect.y + Random.Range(1, rect.height - height);
            rect = new Rect(x, y, width, height);
            
            Managers.Map.roomList.Add(new Room(new Vector3(x,y), width, height, GetRandomRoomType()));

            //DrawRectangle(rect);
            FillRoom(rect);
        }
        else
        {
            tree.leftSpace.roomRect = GenerateRoom(tree.leftSpace, n + 1);
            tree.rightSpace.roomRect = GenerateRoom(tree.rightSpace, n + 1);
            rect = tree.leftSpace.roomRect;
        }
        return rect;
    }

    private RoomType GetRandomRoomType()
    {
        int typeNum = _notGeneratedRoom[Random.Range(0, _notGeneratedRoom.Count)];
        _roomCounts[typeNum]--;
        if (_roomCounts[typeNum] <= 0)
        {
            _notGeneratedRoom.Remove(typeNum);
        }
        return (RoomType)typeNum;
    }

    // 각 방을 이어주는 길 생성
    private void GenerateLoad(SpaceNode tree, int n)
    {
        if (n == _maxDepth)
            return;

        Vector3 leftNodeCenter = tree.leftSpace.center;
        Vector3 rightNodeCenter = tree.rightSpace.center;

        //DrawLine(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));
        //DrawLine(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));
        for (int i = (int)Mathf.Min(leftNodeCenter.x, rightNodeCenter.x); i <= Mathf.Max(leftNodeCenter.x, rightNodeCenter.x); i++)
        {
            _tileMap.SetTile(new Vector3Int(i - _mapSize.x / 2, (int)leftNodeCenter.y - _mapSize.y / 2, 0), roomTile);
        }

        for (int j = (int)Mathf.Min(leftNodeCenter.y, rightNodeCenter.y); j <= Mathf.Max(leftNodeCenter.y, rightNodeCenter.y); j++)
        {
            _tileMap.SetTile(new Vector3Int((int)rightNodeCenter.x - _mapSize.x / 2, j - _mapSize.y / 2, 0), roomTile);
        }

        GenerateLoad(tree.leftSpace, n + 1);
        GenerateLoad(tree.rightSpace, n + 1);
    }
    #endregion

    #region Draw
    // lineRender로 제작할 맵을 그리는 함수
    private void DrawMap(int x, int y)
    {
        LineRenderer lineRenderer = Instantiate(_map).GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.SetPosition(0, new Vector2(x, y) - _mapSize / 2);
        lineRenderer.SetPosition(1, new Vector2(x + _mapSize.x, y) - _mapSize / 2);
        lineRenderer.SetPosition(2, new Vector2(x + _mapSize.x, y + _mapSize.y) - _mapSize / 2);
        lineRenderer.SetPosition(3, new Vector2(x, y + _mapSize.y) - _mapSize / 2);
    }

    // lineRender로 각 영역의 경계선을 그리는 함수
    private void DrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(_line).GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.SetPosition(0, from - _mapSize / 2);
        lineRenderer.SetPosition(1, to - _mapSize / 2);
    }
    
    // lineRender로 방을 그리는 함수
    private void DrawRectangle(Rect rect)
    {
        LineRenderer lineRenderer = Instantiate(_roomLine).GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - _mapSize / 2);
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - _mapSize / 2);
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - _mapSize / 2);
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - _mapSize / 2);
        lineRenderer.SetColors(Color.white, Color.white);
    }
    #endregion

    #region FillTile
    //배경을 채우는 함수
    void FillBackground() 
    {
        for (int i = -10; i < _mapSize.x + 10; i++)
        {
            for (int j = -10; j < _mapSize.y + 10; j++)
            {
                _tileMap.SetTile(new Vector3Int(i - _mapSize.x / 2, j - _mapSize.y / 2, 0), outTile);
            }
        }
    }

    //룸 타일과 바깥 타일이 만나는 부분
    void FillWall() 
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                if (_tileMap.GetTile(new Vector3Int(i - _mapSize.x / 2, j - _mapSize.y / 2, 0)) == outTile)
                {
                    //바깥타일 일 경우
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;
                            if (_tileMap.GetTile(new Vector3Int(i - _mapSize.x / 2 + x, j - _mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                _tileMap.SetTile(new Vector3Int(i - _mapSize.x / 2, j - _mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //방의 Rect정보를 받아서 tile을 설정하는 함수
    private void FillRoom(Rect rect)
    {
        int x = (int)System.Math.Round(rect.x);
        int y = (int)System.Math.Round(rect.y);
        int width = (int)System.Math.Round(rect.width);
        int height = (int)System.Math.Round(rect.height);

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                _tileMap.SetTile(new Vector3Int(i - _mapSize.x / 2, j - _mapSize.y / 2, 0), roomTile);
            }
        }
    }
    #endregion
}