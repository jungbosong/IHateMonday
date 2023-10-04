using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    // ���Ͱ� ������ ��ü Position�� ��
    const int TOTAL_SPAWN_COUNT = 6;

    // Spawn Position�� ������ ���� ��ǥ (���� ��� ��ǥ)
    private Vector3 _centerPos;
    // ���� ��� ��ǥ ~ ������ ����
    private float _limit;

    private int _currentWaveIndex = 0;      // ���� ���̺� index
    private int _currentSpawnCount = 0;     // ���� ���Ͱ� �����Ǵ� spawn�� ���� (���Ͱ� ������ ���� -> ���̺� ���� üũ)
    private int _waveSpawnCount = 0;        // ���̺� �� ���� ��������� ������ ��
    private int _waveSpawnPosCount = 0;     // ���̺꿡�� ���Ͱ� �����Ǵ� spawn�� ��
    private float _spawnInterval = 0.5f;     // ���̺� ����

    private List<Vector3> _spawnPositions = new List<Vector3>();        // ���Ͱ� ������ spawn�� position ����Ʈ

    public List<GameObject> _enemyPrefabs = new List<GameObject>();     // ������ ���� ������ ����Ʈ

    private Room _curRoom;
    private bool isWaveOver = false;

    void Start()
    {
        InitSpawnPositions();
        StartCoroutine("COPlayWave");
    }

    public void InitRoomInfo(Vector3 centerPosition, Room room)
    {
        _centerPos.x = centerPosition.x;
        _centerPos.y = centerPosition.y;
        _limit = (room.width <= room.height) ? room.width / 2 : room.height / 2;    // ���� ����, ���� ���� �� �� ª�� �� ������ ��
        _curRoom = room;
    }

    // ���� ��ǥ�� limit�� ���� ������ spawn position ����Ʈ �ʱ�ȭ
    private void InitSpawnPositions()
    {
        _spawnPositions.Clear();
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

    // ���̺� ���� �ڷ�ƾ �Լ�
    IEnumerator COPlayWave()
    {
        while (!isWaveOver)
        {
            if (_currentSpawnCount == 0)
            {
                if (_currentWaveIndex > 10)
                {
                    CreateReward();
                    isWaveOver = true;
                    (_curRoom.OnBattleEnd)?.Invoke();
                    Managers.Sound.Play("wave_clear");
                    break;
                }

                // ���Ͱ� �����Ǵ� ������ �� ����
                if (_currentWaveIndex % 5 == 0)
                {
                    _waveSpawnPosCount = _waveSpawnPosCount + 1 > _spawnPositions.Count ? _waveSpawnPosCount : _waveSpawnPosCount + 1;
                    _waveSpawnCount = 0;
                }

                // �� ���� ��������� ���� �� ����
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
                        //enemy.transform.localPosition = _spawnPositions[posIdx];
                        //enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(_monsterStats);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
                        _currentSpawnCount++;
                        yield return new WaitForSeconds(_spawnInterval);
                    }
                }
                _currentWaveIndex++;
            }
            yield return null;
        }
    }

    // ������ OnDeath �̺�Ʈ�� �����
    private void OnEnemyDeath()
    {
        _currentSpawnCount--;
    }

    // ���̺� Ŭ���� �� ���� ���� ����
    private void CreateReward()
    {
        Debug.Log("����");
        // 50�ۼ�Ʈ Ȯ���� ���� ����
        if (IsPossible(50))
        {
            Managers.Resource.Instantiate("Items/Key", new Vector3(_centerPos.x, _centerPos.y, 0));
        }
    }

    private bool IsPossible(int percent)
    {
        int num = Random.Range(0, 100);
        return (num < percent) ? true : false;
    }
}
