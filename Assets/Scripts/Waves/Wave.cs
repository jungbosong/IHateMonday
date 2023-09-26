using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private int _currentWaveIndex;
    private int _currentSpawnCount;
    private int _waveSpawnCount;
    private int _waveSpawnPosCount;
    private float _spawnInterval;

    public List<GameObject> _enemyPrefabs = new List<GameObject>();
    private List<Transform> _spawnPositions = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _spawnPositions.Add(transform.GetChild(i));
        }
    }

    void Start()
    {
        StartCoroutine("COPlayWave");
    }

    IEnumerator COPlayWave()
    {
        while (_currentWaveIndex <= 5)
        {
            if (_currentSpawnCount == 0)
            {
                // 한 번에 만들어지는 몬스터 수 증가
                if (_currentWaveIndex % 2 == 0)
                {
                    _waveSpawnPosCount++;
                }

                // 몬스터가 생성되는 포지션 수 증가
                if (_currentWaveIndex % 5 == 0)
                {
                    _waveSpawnPosCount = _waveSpawnPosCount + 1 > _spawnPositions.Count ? _waveSpawnPosCount : _waveSpawnPosCount + 1;
                }

                for (int i = 0; i < _waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, _spawnPositions.Count);
                    for (int j = 0; j < _waveSpawnCount; j++)
                    {
                        int prefabIndex = Random.Range(0, _enemyPrefabs.Count);
                        GameObject enemy = Instantiate(_enemyPrefabs[prefabIndex], _spawnPositions[posIdx].position, Quaternion.identity);
                        _currentSpawnCount++;
                        yield return new WaitForSeconds(_spawnInterval);
                    }
                }
                _currentWaveIndex++;

                Debug.Log(_currentWaveIndex);
            }
            yield return null;
        }
    }
}
