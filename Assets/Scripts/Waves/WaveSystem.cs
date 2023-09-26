using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    const string WAVE_PREFAB_PATH = "Prefabs/Waves/Stage";

    private string _dataPath;
    private int _curStage;
    private int _curRoomNum;
    private bool _isBossRoom;

    private List<GameObject> _waveList = new List<GameObject>();
    private List<BossStage> _bossStageList;

    private void Start()
    {
        _curStage = 1;
        InitWaveList();
        StartWave(1, false);
    }

    private void InitWaveList()
    {
        _dataPath = WAVE_PREFAB_PATH + _curStage;
        GameObject[] objs = Resources.LoadAll<GameObject>(_dataPath);

        for (int i = 0; i < objs.Length; i++)
        {
            _waveList.Add(Instantiate(objs[i]));
        }
    }

    public void StartWave(int roomNum, bool isBossRoom)
    {
        //if (isBossRoom)
        //{

        //} else
        //{
            _waveList[roomNum - 1].SetActive(true);
        //}
    }
}
