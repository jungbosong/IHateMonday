using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // wave 프리팹의 경로
    const string WAVE_PREFAB_PATH = "Prefabs/Waves/Wave";
    const string BOSS_STAGE_PREFAB_PATH = "Prefabs/Waves/BossStage";

    // 최종 스테이지 번호
    const int MAX_STAGE_NUM = 3;

    private int _curStage;
    private Room _currentRoom;
    private Wave _curWave;
    private BossStage _curBossStage;

    // Player
    public GameObject player;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        _healthSystem = player.GetComponent<HealthSystem>();
        _healthSystem.OnDeath += GameOver;
    }

    void Start()
    {
        _curStage = 1;
        // 웨이브 테스트용 코드 
        //_currentRoom = new Room(new Vector3(2f, 2f, 0), 5f, 7f, RoomType.Boss);
        //StartWave();
    }

    void Update()
    {
        
    }

    // 게임 매니저에게 웨이브 시작을 요청할 때 사용
    // 플레이어가 들어간 방의 정보를 Room 타입으로 넘겨줘야 함
    public void StartWave(Room curRoom)
    {
        _currentRoom = curRoom;

        if (_currentRoom.type == RoomType.Wave)
        {
            _curWave = Instantiate(Resources.Load<GameObject>(WAVE_PREFAB_PATH)).GetComponent<Wave>();
            _curWave.InitRoomInfo(_currentRoom);
        }
        else
        {
            _curBossStage = Instantiate(Resources.Load<GameObject>(BOSS_STAGE_PREFAB_PATH)).GetComponent<BossStage>();
            _curBossStage.InitRoomInfo(_currentRoom, _curStage);
        }
    }

    // 보스 스테이지 클리어시 BossStage에서 호출
    public void StageClear()
    {
        if (_curStage == MAX_STAGE_NUM)
        {
            // 게임 최종 클리어 엔딩씬으로 넘어가기
            Managers.Scene.ChangeScene(Define.Scene.EndingScene);
        }
        else
        {
            _curStage++;
        }
    }

    // 플레이어가 죽었을 때의 게임 오버 처리
    private void GameOver()
    {
        StopAllCoroutines();
        _curStage = 1;
        // 실패 엔딩씬으로 넘어가기
        Managers.Scene.ChangeScene(Define.Scene.DeadEndScene);
    }
}
