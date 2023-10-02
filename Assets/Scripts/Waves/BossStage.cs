using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    private int _curStage;

    // Spawn Position이 생성될 기준 좌표 (방의 가운데 좌표)
    private Vector3 _centerPos;
    // 방의 가운데 좌표 ~ 벽까지 길이
    private float _limit;

    private bool _isGameOver;

    private List<Vector3> _spawnPositions = new List<Vector3>();        // 몬스터가 생성될 spawn의 position 리스트

    public List<GameObject> _bossPrefabs = new List<GameObject>();     // 생성할 보스 몬스터 프리팹 리스트

    void Start()
    {
        _isGameOver = false;
        InitSpawnPositions();
        StartBossWave();
    }

    void Update()
    {
        if (_isGameOver)
        {
            Managers.Game.StageClear();
        }
    }

    public void InitRoomInfo(Room room, int curStage)
    {
        _centerPos.x = room.center.x;
        _centerPos.y = room.center.y;
        _limit = (room.width <= room.height) ? room.width / 2 : room.height / 2;    // 방의 가로, 세로 길이 중 더 짧은 쪽 길이의 반
        _curStage = curStage;
    }

    // 방의 좌표와 limit에 따라 몬스터의 spawn position 리스트 초기화
    private void InitSpawnPositions()
    {
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j++)
            {
                float x = _centerPos.x + (j * _limit * 0.8f);
                float y = _centerPos.y + (i * _limit * 0.8f);
                _spawnPositions.Add(new Vector3(x, y));
            }
        }
    }

    private void StartBossWave()
    {
        int posIdx = Random.Range(0, _spawnPositions.Count);
        GameObject boss = Instantiate(_bossPrefabs[_curStage - 1], _spawnPositions[posIdx], Quaternion.identity);
        boss.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
    }

    // 몬스터의 OnDeath 이벤트에 연결됨
    private void OnEnemyDeath()
    {
        _isGameOver = true;
    }
}
