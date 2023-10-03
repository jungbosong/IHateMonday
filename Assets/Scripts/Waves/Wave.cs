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

    private int _currentWaveIndex = 0;      // 현재 웨이브 index
    private int _currentSpawnCount = 0;     // 현재 몬스터가 생성되는 spawn의 개수 (몬스터가 죽으면 감소 -> 웨이브 종료 체크)
    private int _waveSpawnCount = 0;        // 웨이브 한 번에 만들어지는 몬스터의 수
    private int _waveSpawnPosCount = 0;     // 웨이브에서 몬스터가 생성되는 spawn의 수
    private float _spawnInterval = 0.5f;     // 웨이브 간격

    private List<Vector3> _spawnPositions = new List<Vector3>();        // 몬스터가 생성될 spawn의 position 리스트

    public List<GameObject> _enemyPrefabs = new List<GameObject>();     // 생성할 몬스터 프리팹 리스트

    private CharacterStats _monsterStats;
    //private Room _room;

    void Start()
    {
        InitSpawnPositions();
        InitUpgradeStat();
        StartCoroutine("COPlayWave");
    }

    public void InitRoomInfo(Vector3 centerPosition, Room room)
    {
        _centerPos.x = centerPosition.x;
        _centerPos.y = centerPosition.y;
        _limit = (room.width <= room.height) ? room.width / 2 : room.height / 2;    // 방의 가로, 세로 길이 중 더 짧은 쪽 길이의 반
        //_room = room;
    }

    // 방의 좌표와 limit에 따라 몬스터의 spawn position 리스트 초기화
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
                    if(_monsterStats != null) RandomUpgrade();
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
        CreateReward();
    }

    // TODO 아이템 부분 완성되면 CharacterStats 리팩토링하고 주석 풀기
    private void InitUpgradeStat()
    {
        //_monsterStats.statsChangeType = StatsChangeType.Add;
    }

    // 몬스터의 OnDeath 이벤트에 연결됨
    private void OnEnemyDeath()
    {
        _currentSpawnCount--;
    }

    // 웨이브 클리어 시 보상 제공
    private void CreateReward()
    {
        // TODO 보상 아이템 생성
        // 현재 웨이브 방에서 진행되는 웨이브는 총 10회 
        // 10회가 끝나야 보상을 제공할 것인지, 중간에도 제공할 것인지 결정해야 함 -> 밸런스 패치와 연관
        //Instantiate();
    }

    // TODO 랜덤하게 몬스터의 스탯을 변화시키는 로직 구현
    // 고려할 점 : PlayerStatsHandler에 있는 StatModifier 방식을 CharacterStatsHandler로 옮기고 MonsterStatsHandler 추가로 생성
    // PlayerStatsHandler와 MonsterStatsHandler에서는 넘겨 받은 CharaterStats를 타입캐스팅하여 스탯 적용 -> 효율적인가?
    private void RandomUpgrade()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                _monsterStats.currentHp += 3;
                break;
            case 1:
                _monsterStats.currentHp += 6;
                break;
            case 2:
                _monsterStats.currentHp += 9;
                break;
            case 3:
                _monsterStats.moveSpeed += 1;
                break;
            case 4:
                _monsterStats.moveSpeed += 3;
                break;
            case 5:
                _monsterStats.moveSpeed += 5;
                break;
            default:
                break;
        }
    }
}
