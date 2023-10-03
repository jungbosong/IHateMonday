using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private const float CO_TIME = 0.1f;

    private PlayerStats _playerStats;
    private PlayerStats _changeInvincibilityStats;
    [SerializeField]
    private PlayerStats _changeIncreaseStats;
    [SerializeField]
    private PlayerStats _changeGuiedStats;
    private PlayerStatsHandler _controller;
    private GameObject[] _bullet;
    private float _buffTime = 10.0f;

    private void Start()
    {
        _controller = GetComponent<PlayerStatsHandler>();
        _playerStats = _controller.CurrentStats;
    }

    public void OnGuied()
    {
        //gun 발사방식 타겟 찾아 n발 날라가기 시간.or 탄피
        //플레이어 bool 변수 변경
        _changeGuiedStats = _controller.CurrentStats;
        _changeGuiedStats.isGuied = true;
        _changeGuiedStats.statsChangeType = StatsChangeType.Override;
        _controller.AddStatModifier(_changeGuiedStats);
    }

    public void OffGuied()
    {
        _changeGuiedStats = _controller.CurrentStats;
        _changeGuiedStats.isGuied = false;
        _changeIncreaseStats.statsChangeType= StatsChangeType.Override;
        _controller.AddStatModifier(_changeGuiedStats);
    }

    public void OnDamageIncrease()
    {
        //10초간 player damage 증가
        StartCoroutine(COOnIncreaseDamage());
    }

    public void OnDestroyBullet()
    {
        _bullet = GameObject.FindGameObjectsWithTag("Bullet");
        
        foreach(GameObject currentAllBullet in _bullet)
        {
            Managers.Resource.Destroy(currentAllBullet);
        }
    }

    public void OnInvincibilite()
    {
        //2초간 무적
        StartCoroutine(COOnInvincibility());
    }

    IEnumerator COOnInvincibility()
    {
        _changeInvincibilityStats = _controller.CurrentStats;
        _changeInvincibilityStats.isInvincible = true;
        _changeInvincibilityStats.statsChangeType = StatsChangeType.Override;    // 무적인 상태로 변환
        _controller.AddStatModifier(_changeInvincibilityStats);
        while(_changeInvincibilityStats.invincibilityTime > 0)
        {
            _changeInvincibilityStats.invincibilityTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }

        _playerStats = _controller.CurrentStats;
        _playerStats.statsChangeType = StatsChangeType.Override;    // 무적 끝나고 스탯 복귀
        _controller.AddStatModifier(_playerStats);
    }

    IEnumerator COOnIncreaseDamage()
    {
        _changeIncreaseStats.attackPowerCoefficient = 1.5f;
        _changeIncreaseStats.statsChangeType = StatsChangeType.Multiple;    //공격력 150% -> 1.5

        _controller.AddStatModifier(_changeIncreaseStats);
        while (_buffTime > 0)
        {
            _buffTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }
        _playerStats = _controller.CurrentStats;    //스탯 복귀
        _playerStats.statsChangeType = StatsChangeType.Override;
        _controller.AddStatModifier(_playerStats);
    }
}
