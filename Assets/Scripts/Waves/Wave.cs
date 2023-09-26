using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    // 몬스터가 생성될 전체 Position의 수
    const int TOTAL_SPAWN_COUNT = 6;

    // Spawn Position이 생성될 기준 좌표 (방의 가운데 좌표)
    private Vector3 _centerPos;
    // 방의 가운데 좌표 ~ 벽까지 길이
    private float _limit;

    #region Wave variable
    private int _currentWaveIndex = 0;      // 현재 웨이브 index
    private int _currentSpawnCount = 0;     // 현재 몬스터가 생성되는 spawn의 개수 (몬스터가 죽으면 감소 -> 웨이브 종료 체크)
    private int _waveSpawnCount = 0;        // 웨이브 한 번에 만들어지는 몬스터의 수
    private int _waveSpawnPosCount = 0;     // 웨이브에서 몬스터가 생성되는 spawn의 수
    private float _spawnInterval = 0.5f;     // 웨이브 간격
    #endregion

    public List<GameObject> _enemyPrefabs = new List<GameObject>();     // 생성할 몬스터 프리팹 리스트
    private List<Vector3> _spawnPositions = new List<Vector3>();        // 몬스터가 생성될 spawn의 position 리스트

    private void Awake()
    {
        InitSpawnPositions();
    }

    void Start()
    {
        StartCoroutine("COPlayWave");
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

    public void SetSpawnPositionInfo(float x, float y, float limit)
    {
        _centerPos.x = x;
        _centerPos.y = y;
        _limit = limit;
    }

    // 웨이브 실행 코루틴 함수
    IEnumerator COPlayWave()
    {
        while (_currentWaveIndex <= 10)
        {
            if (_currentSpawnCount == 0)
            {
                // 몬스터가 생성되는 포지션 수 증가
                if (_currentWaveIndex % 5 == 0)
                {
                    _waveSpawnPosCount = _waveSpawnPosCount + 1 > _spawnPositions.Count ? _waveSpawnPosCount : _waveSpawnPosCount + 1;
                    _waveSpawnCount = 0;
                }

                // 한 번에 만들어지는 몬스터 수 증가
                if (_currentWaveIndex % 2 == 0)
                {
                    _waveSpawnCount++;
                }

                for (int i = 0; i < _waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, _spawnPositions.Count);
                    for (int j = 0; j < _waveSpawnCount; j++)
                    {
                        int prefabIndex = Random.Range(0, _enemyPrefabs.Count);
                        GameObject enemy = Instantiate(_enemyPrefabs[prefabIndex], _spawnPositions[posIdx], Quaternion.identity);
                        // TODO Monster 스탯 변경
                        // TODO Monster Death 처리 함수 Event에 연결
                        _currentSpawnCount++;
                        yield return new WaitForSeconds(_spawnInterval);
                    }
                }
                _currentWaveIndex++;
            }
            yield return null;
        }
    }

    // TODO Monster Death 처리 함수 추가
}
