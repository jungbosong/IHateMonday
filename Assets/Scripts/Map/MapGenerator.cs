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
    [SerializeField] private int _maxDepth;             // 영역을 자세하게 나누는 정도(깊이)
    #endregion

    #region lineRenderer
    [SerializeField] private GameObject _line;          // 각 영역의 경계선을 표시하기 위한 line renderer
    [SerializeField] private GameObject _map;           // root 영역을 표시하기 위한 line renderer
    [SerializeField] private GameObject _roomLine;      // 방을 표시하기 위한 line renderer
    #endregion

    void Start()
    {
        SpaceNode root = new SpaceNode(new RectInt(0, 0, _mapSize.x, _mapSize.y));
        DrawMap(0, 0);
        Divide(root, 0);
        GenerateRoom(root, 0);
    }

    #region GenerateMap
    // 방을 n만큼의 깊이로 이진 분할
    private void Divide(SpaceNode tree, int n)
    {
        if (n == _maxDepth) return;

        int maxLength = Mathf.Max(tree.spaceRect.width, tree.spaceRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength * _minDevideRate, maxLength *_maxDevideRate));
        if(tree.spaceRect.width >= tree .spaceRect.height)
        {
            tree.leftSpace = new SpaceNode(new RectInt(tree.spaceRect.x, tree.spaceRect.y, split, tree.spaceRect.height));
            tree.rightSpace = new SpaceNode(new RectInt(tree.spaceRect.x + split, tree.spaceRect.y, tree.spaceRect.width - split, tree.spaceRect.height));
            DrawLine(new Vector2(tree.spaceRect.x + split, tree.spaceRect.y), new Vector2(tree.spaceRect.x + split, tree.spaceRect.y + tree.spaceRect.height));
        }
        else
        {
            tree.leftSpace = new SpaceNode(new RectInt(tree.spaceRect.x, tree.spaceRect.y, tree.spaceRect.width, split));
            tree.rightSpace = new SpaceNode(new RectInt(tree.spaceRect.x, tree.spaceRect.y + split, tree.spaceRect.width, tree.spaceRect.height - split));
            DrawLine(new Vector2(tree.spaceRect.x , tree.spaceRect.y+ split), new Vector2(tree.spaceRect.x + tree.spaceRect.width, tree.spaceRect.y  + split));
        }
        tree.leftSpace.parentSpace = tree;
        tree.rightSpace.parentSpace = tree;
        Divide(tree.leftSpace, n + 1);
        Divide(tree.rightSpace, n + 1);
    }

    // 리프영역에 해당 영역 보다 작은 사이즈의 방 제작
    private RectInt GenerateRoom(SpaceNode tree, int n)
    {
        RectInt rect;
        if (n == _maxDepth)
        {
            rect = tree.spaceRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);
            DrawRectangle(rect);
        }
        else
        {
            tree.leftSpace.roomRect = GenerateRoom(tree.leftSpace, n + 1);
            tree.rightSpace.roomRect = GenerateRoom(tree.rightSpace, n + 1);
            rect = tree.leftSpace.roomRect;
        }
        return rect;
    }
    #endregion

    #region Draw
    // lineRender로 제작할 맵을 그리는 함수
    private void DrawMap(int x, int y)
    {
        LineRenderer lineRenderer = Instantiate(_map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - _mapSize / 2); //좌측 하단
        lineRenderer.SetPosition(1, new Vector2(x + _mapSize.x, y) - _mapSize / 2); //우측 하단
        lineRenderer.SetPosition(2, new Vector2(x + _mapSize.x, y + _mapSize.y) - _mapSize / 2);//우측 상단
        lineRenderer.SetPosition(3, new Vector2(x, y + _mapSize.y) - _mapSize / 2);
    }

    // lineRender로 각 영역의 경계선을 그리는 함수
    private void DrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(_line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - _mapSize / 2);
        lineRenderer.SetPosition(1, to - _mapSize / 2);
    }
    
    // lineRender로 방을 그리는 함수
    private void DrawRectangle(RectInt rect)
    {
        LineRenderer lineRenderer = Instantiate(_roomLine).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - _mapSize / 2);
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - _mapSize / 2);
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - _mapSize / 2);
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - _mapSize / 2);
        lineRenderer.SetColors(Color.white, Color.white);
    }
    #endregion
}