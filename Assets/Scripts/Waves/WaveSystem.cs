using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // wave 프리팹의 경로
    const string WAVE_PREFAB_PATH = "Prefabs/Waves/Wave";

    private string _dataPath;   // 현재 스테이지 정보를 받아와서 완성하는 최종 프리팹 경로
    private int _curStage;      // 현재 스테이지
    private GameObject _wavePrefab;

    private void Start()
    {
        _dataPath = WAVE_PREFAB_PATH + _curStage;
        _wavePrefab = Resources.Load<GameObject>(_dataPath);
    }

    public void StartWave(bool isBossRoom, float x, float y, float limit)
    {
        //if (isBossRoom)
        //{
        //      
        //} else
        //{
        //  _wavePrefab.GetComponent<Wave>().SetSpawnPositionInfo(x, y, limit);
        //  Instantiate(_wavePrefab);
        //}
    }
}
