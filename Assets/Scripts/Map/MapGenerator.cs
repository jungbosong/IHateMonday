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
    [SerializeField] private GameObject _line;          // 나뉜 영역을 선으로 보여주기 위함
    [SerializeField] private GameObject _map;           // line renderer로 표시되는 root 영역
    #endregion

    void Start()
    {
        SpaceNode root = new SpaceNode(new RectInt(0, 0, _mapSize.x, _mapSize.y));
        DrawMap(0, 0);
        Divide(root, 0);
    }

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
}