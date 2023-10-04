using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // wave �������� ���
    const string WAVE_PREFAB_PATH = "Prefabs/Waves/Wave";
    const string BOSS_STAGE_PREFAB_PATH = "Prefabs/Waves/BossStage";

    // ���� �������� ��ȣ
    const int MAX_STAGE_NUM = 3;

    private int _curStage;
    private Room _currentRoom;
    private Wave _curWave;
    private BossStage _curBossStage;

    // Player
    public GameObject player;
    private PlayerStatsHandler _playerStatsHandler;
    private HealthSystem _healthSystem;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        _playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        _healthSystem = player.GetComponent<HealthSystem>();
        _healthSystem.OnDeath += GameOver;

        _curStage = 1;
    }

    public void Start()
    {
        // 웨이브 시스템 테스트용 코드
        //_currentRoom = new Room(new Vector3(2f, 2f, 0), 5f, 7f, RoomType.Wave);
        //StartWave(_currentRoom);
    }

    void Update()
    {
        
    }

    // 사용자가 들어간 Room 정보를 넘겨 받아서 Wave 시작
    public void StartWave(Vector3 centerPosition, Room curRoom)
    {
        _currentRoom = curRoom;

        if (_currentRoom.type == RoomType.Wave)
        {
            _curWave = Instantiate(Resources.Load<GameObject>(WAVE_PREFAB_PATH)).GetOrAddComponent<Wave>();
            //_curWave.transform.position = curRoom.transform.localPosition;
            _curWave.InitRoomInfo(centerPosition, _currentRoom);
        }
        else
        {
            _curBossStage = Instantiate(Resources.Load<GameObject>(BOSS_STAGE_PREFAB_PATH)).GetOrAddComponent<BossStage>();
            //_curBossStage.transform.position = curRoom.transform.localPosition;
            _curBossStage.InitRoomInfo(centerPosition, _currentRoom, _curStage);
        }
    }

    // 보스 스테이지 클리어 시 현재 스테이지 값 증가
    public void StageClear()
    {
        if (_curStage == MAX_STAGE_NUM)
        {
            // 최종 스테이지면 엔딩 씬으로 이동
            Managers.Scene.ChangeScene(Define.Scene.EndingScene);
        }
        else
        {
            _curStage++;
        }
    }

    // 플레이어가 죽으면 게임 오버
    private void GameOver()
    {
        _curStage = 1;
        // 게임 실패 씬으로 이동
        Managers.Scene.ChangeScene(Define.Scene.DeadEndScene);
    }
}
