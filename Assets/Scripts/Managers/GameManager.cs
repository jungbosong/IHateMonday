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

    private UI_DungeonScene _dungeonUI;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player.name);
        _playerStatsHandler = player.GetComponent<PlayerStatsHandler>();
        _healthSystem = player.GetComponent<HealthSystem>();
        _healthSystem.OnDeath += GameOver;

        //GameObject uiRoot = Managers.UI.Root;
        //_dungeonUI = uiRoot.transform.Find("UI_DungeonScene").GetComponent<UI_DungeonScene>();
        //_dungeonUI.UpdatePlayerStatUI(_playerStatsHandler.CurrentStats);
    }

    public void Start()
    {
        _curStage = 1;

        // ���̺� �׽�Ʈ�� �ڵ� 
        //_currentRoom = new Room(new Vector3(2f, 2f, 0), 5f, 7f, RoomType.Wave);
        //StartWave(_currentRoom);
    }

    void Update()
    {
        
    }

    // ���� �Ŵ������� ���̺� ������ ��û�� �� ���
    // �÷��̾ �� ���� ������ Room Ÿ������ �Ѱ���� ��
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

    // ���� �������� Ŭ����� BossStage���� ȣ��
    public void StageClear()
    {
        if (_curStage == MAX_STAGE_NUM)
        {
            // ���� ���� Ŭ���� ���������� �Ѿ��
            Managers.Scene.ChangeScene(Define.Scene.EndingScene);
        }
        else
        {
            _curStage++;
        }
    }

    // �÷��̾ �׾��� ���� ���� ���� ó��
    private void GameOver()
    {
        StopAllCoroutines();
        _curStage = 1;
        // ���� ���������� �Ѿ��
        Managers.Scene.ChangeScene(Define.Scene.DeadEndScene);
    }
}
