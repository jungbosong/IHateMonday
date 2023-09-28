using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using System;
using System.Linq;
//using System.Xml.Linq;
//using System.Reflection;
//using UnityEditor.PackageManager.UI;

public class CharacterStatsHandler : MonoBehaviour
{
    private int MAX_HP = 8;                         // 최대 보유 체력
    private const int MAX_SHIELD_COUNT = 2;         // 최대 보유 쉴드
    private const float MAX_ATTACK_POWER = 200f;    // 최대 공격력 증가 계수
    private const float MAX_ATTACK_SPEED = 200f;    // 최대 공격속도 증가 계수
    private const float MAX_MOVE_SPEED = 200f;      // 최대 이동속도 증가 계수

    [SerializeField] private CharacterStats baseStats;

    public CharacterStats CurrentStats { get; private set; }

    public List<CharacterStats> statsModifiers = new List<CharacterStats>();

    //public CharacterStats _playerStat;
    //private CharacterStatsHandler _characterStatsHandler;
    //_characterStatsHandler = GetComponent<CharacterStatsHandler>();
    // 스탯변경을 위해서 위 두줄이 필요함
    //사용 방법 : _characterStatsHandler._playerStat.Get or Set method(float 변화값, int 스탯 타입);

    private void Awake()
    {
        UpdateCharacterStats();
    }

    public void AddStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Add(statModifier);
        UpdateCharacterStats();
    }

    public void RemoveStatModifier(CharacterStats statModifier)
    {
        statsModifiers.Remove(statModifier);
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats()
    {
        CurrentStats = new CharacterStats();  // 생성하면서 초기화는 중괄호
        UpdateStats((a, b) => b, baseStats);    //a, b를 받아서 후자를 사용 -> CurrentStat에 baseStat을 덮어씌움

        foreach (CharacterStats modifier in statsModifiers.OrderBy(o => o.statsChangeType))
        {
            if (modifier.statsChangeType == StatsChangeType.Override)
            {
                UpdateStats((o, o1) => o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Add)
            {
                UpdateStats((o, o1) => o + o1, modifier);
            }
            else if (modifier.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStats((o, o1) => o * o1, modifier);
            }
        }

        LimitAllStats();
    }

    private void UpdateStats(Func<float, float, float> operation, CharacterStats newModifier)
    {
        CurrentStats.currentMaxHp = (int)operation(CurrentStats.currentMaxHp, newModifier.currentMaxHp);
        CurrentStats.currentHp = (int)operation(CurrentStats.currentHp, newModifier.currentHp);
        CurrentStats.shieldCount = (int)operation(CurrentStats.shieldCount, newModifier.shieldCount);
        CurrentStats.attackPowerCoefficient = operation(CurrentStats.attackPowerCoefficient, newModifier.attackPowerCoefficient);
        CurrentStats.attackSpeedCoefiicient = operation(CurrentStats.moveSpeed, newModifier.attackSpeedCoefiicient);
        CurrentStats.moveSpeed = operation(CurrentStats.moveSpeed, newModifier.moveSpeed);
    }

    private void LimitAllStats()
    {
        LimitStats(ref CurrentStats.currentMaxHp, MAX_HP);
        LimitStats(ref CurrentStats.currentHp, CurrentStats.currentMaxHp);
        LimitStats(ref CurrentStats.shieldCount, MAX_SHIELD_COUNT);
        LimitStats(ref CurrentStats.attackPowerCoefficient, MAX_ATTACK_POWER);
        LimitStats(ref CurrentStats.attackSpeedCoefiicient, MAX_ATTACK_SPEED);
        LimitStats(ref CurrentStats.moveSpeed, MAX_MOVE_SPEED);
    }

    private void LimitStats(ref int stat, int minVal)
    {
        stat = Mathf.Min(stat, minVal);
    }

    private void LimitStats(ref float stat, float minVal)
    {
        stat = Mathf.Min(stat, minVal);
    }

    //public void ChangeStat(float change, int type)
    //{
    //    switch (type)
    //    {
    //        case 1: //현재 체력
    //            _playerStat.SetCurrentHp((int)change); break;
    //        case 2: //현재 최대체력
    //            _playerStat.SetCurrentMaxHp((int)change); break;
    //        case 3: //보유 쉴드 개수
    //            _playerStat.SetShieldCount((int)change); break;
    //        case 4: //공격력 증가 계수
    //            _playerStat.SetAttackPowerCoefficient(change); break;
    //        case 5: //공격속도 증가 계수
    //            _playerStat.SetAttackSpeedCoefiicient(change); break;
    //        case 6: //이동속도
    //            _playerStat.SetMoveSpeed(change); break;
    //        case 7: //이동속도 증가 계수
    //            _playerStat.SetMoveSpeedCoefficient(change); break;
    //    }
    //}
}
