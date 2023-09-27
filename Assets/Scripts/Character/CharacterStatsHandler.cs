using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using System;
//using System.Xml.Linq;
//using System.Reflection;
//using UnityEditor.PackageManager.UI;

public class CharacterStatsHandler : MonoBehaviour
{
    public CharacterStats _playerStat;
    //private CharacterStatsHandler _characterStatsHandler;
    //_characterStatsHandler = GetComponent<CharacterStatsHandler>();
    // 스탯변경을 위해서 위 두줄이 필요함
    //사용 방법 : _characterStatsHandler._playerStat.Get or Set method(float 변화값, int 스탯 타입);

    public void ChangeStat(float change, int type)
    {
        switch (type)
        {
            case 1: //현재 체력
                _playerStat.SetCurrentHp((int)change); break;
            case 2: //현재 최대체력
                _playerStat.SetCurrentMaxHp((int)change); break;
            case 3: //보유 쉴드 개수
                _playerStat.SetShieldCount((int)change); break;
            case 4: //공격력 증가 계수
                _playerStat.SetAttackPowerCoefficient(change); break;
            case 5: //공격속도 증가 계수
                _playerStat.SetAttackSpeedCoefiicient(change); break;
            case 6: //이동속도
                _playerStat.SetMoveSpeed(change); break;
            case 7: //이동속도 증가 계수
                _playerStat.SetMoveSpeedCoefficient(change); break;
        }
    }

    
    
    
}
