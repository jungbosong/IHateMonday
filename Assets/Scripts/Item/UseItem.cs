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
        //gun �߻��� Ÿ�� ã�� n�� ���󰡱� �ð�.or ź��
        //�÷��̾� bool ���� ����
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
        //10�ʰ� player damage ����
        StartCoroutine(COOnIncreaseDamage());
        //n�ʰ� player damage ����
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
        //2�ʰ� ����
        StartCoroutine(COOnInvincibility());
    }

    IEnumerator COOnInvincibility()
    {
        _changeInvincibilityStats = _controller.CurrentStats;
        _changeInvincibilityStats.isInvincible = true;
        _changeInvincibilityStats.statsChangeType = StatsChangeType.Override;    // ������ ���·� ��ȯ
        _controller.AddStatModifier(_changeInvincibilityStats);
        while(_changeInvincibilityStats.invincibilityTime > 0)
        {
            _changeInvincibilityStats.invincibilityTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }

        _playerStats = _controller.CurrentStats;
        _playerStats.statsChangeType = StatsChangeType.Override;    // ���� ������ ���� ����
        _controller.AddStatModifier(_playerStats);
    }

    IEnumerator COOnIncreaseDamage()
    {
        float playerAttackPower = _controller.CurrentStats.attackPowerCoefficient;
        _changeIncreaseStats.attackPowerCoefficient = 150f;
        _changeIncreaseStats.statsChangeType = StatsChangeType.Override;    //���ݷ� 150% -> 1.5

        _controller.AddStatModifier(_changeIncreaseStats);
        while (_buffTime > 0)
        {
            _buffTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }
        _playerStats.attackPowerCoefficient = playerAttackPower;    //���� ����
        _playerStats.statsChangeType = StatsChangeType.Override;
        _controller.AddStatModifier(_playerStats);
        //n�ʰ� ����
    }
}
