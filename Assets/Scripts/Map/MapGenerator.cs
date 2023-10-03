using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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
    [SerializeField] private MoveCamera _mainCamera;

    #region lineRenderer
    [SerializeField] private GameObject _line;          // 각 영역의 경계선을 표시하기 위한 line renderer
    [SerializeField] private GameObject _map;           // root 영역을 표시하기 위한 line renderer
    [SerializeField] private GameObject _roomLine;      // 방을 표시하기 위한 line renderer
    #endregion
    TileMapPainter _mapPainter;
    

    void Awake()
    {
        _mapPainter = this.GetComponent<TileMapPainter>();
        _mainCamera = _mainCamera.GetComponent<MoveCamera>();
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
        _mapPainter.FillBackground(_mapSize);
        SpaceNode root = new SpaceNode(new Rect(0, 0, _mapSize.x, _mapSize.y));
        //DrawMap(0, 0);
        Divide(root, 0);
        GenerateRoom(root, 0);
        GenerateRoad(root, 0);
        GenerateDoor(root, 0);
        _mapPainter.FillWall(_mapSize);
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

            RoomType type = GetRandomRoomType();
            Room room = new Room(new Vector3(rect.center.x, rect.center.y, 0), width, height, type);
            Managers.Map.roomList.Add(room);
            if(type == RoomType.NoneMonster)
            {
                GameObject player = Managers.Resource.Instantiate("Characters/Player", new Vector3(rect.center.x - _mapSize.x/2, rect.center.y - _mapSize.y/2, 0));
                _mainCamera.SetTarget(player);
            }
            //DrawRectangle(rect);
            _mapPainter.FillRoom(_mapSize, rect);
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
    private void GenerateRoad(SpaceNode tree, int n)
    {
        if (n == _maxDepth)
            return;

        _mapPainter.FillRoad(tree.leftSpace, tree.rightSpace, _mapSize);
        //DrawLine(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));
        //DrawLine(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));

        GenerateRoad(tree.leftSpace, n + 1);
        GenerateRoad(tree.rightSpace, n + 1);
    }

    private void GenerateDoor(SpaceNode tree, int n)
    {
        if (n == _maxDepth) 
            return;

        SpaceNode leftNode = tree.leftSpace;
        SpaceNode rightNode = tree.rightSpace;

        int startX = (int)System.Math.Round(System.Math.Min(leftNode.center.x, rightNode.center.x));  // 왼쪽에 있는 방의 x좌표
        int startY = (int)System.Math.Round(System.Math.Min(leftNode.center.y, rightNode.center.y));  // 아래쪽에 있는 방의 y좌표

        int endX = (int)System.Math.Round(System.Math.Max(leftNode.center.x, rightNode.center.x));    // 오른쪽에 있는 방의 x좌표
        int endY = (int)System.Math.Round(System.Math.Max(leftNode.center.y, rightNode.center.y));    // 위쪽에 있는 방의 y좌표

        // 세로로 긴 길일 경우
        if(Mathf.Abs(startX-endX) < Mathf.Abs(startY - endY))
        {
            // 길의 시작지점의 y부터 종료지점의 y까지 돌면서
            for (int y = startY; y <= endY; y++)
            {
                // y좌표가 방을 벗어난 위치에 있으면
                if (Mathf.Abs(y) > Mathf.Abs((int)System.Math.Round(rightNode.center.y + rightNode.roomRect.height/2) - _mapSize.y / 2))
                {
                    // 문생성
                    Utils.CreateDoor(new Vector3(startX - _mapSize.x / 2, (int)System.Math.Round(rightNode.center.y - rightNode.roomRect.height / 2) - _mapSize.y / 2, 0), true);
                    break;
                }
            }
        }
        // 가로로 긴 길일 경우
        else
        {
            // 길의 시작지점의 x부터 종료지점의 x까지 돌면서
            for (int x = startX; x <= endX; x++)
            {
                // x좌표가 방을 벗어난 위치에 있으면
                if (Mathf.Abs(x) > Mathf.Abs((int)System.Math.Round(leftNode.center.x + leftNode.roomRect.width/2 - _mapSize.x/2)))
                {
                    // 문생성
                    Utils.CreateDoor(new Vector3((int)System.Math.Round(leftNode.center.x + leftNode.roomRect.width / 2) - _mapSize.x / 2, startY - _mapSize.y / 2, 0), true);
                    break;
                }
            }
        }

        GenerateDoor(tree.leftSpace, n + 1);
        GenerateDoor(tree.rightSpace, n + 1);
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
}