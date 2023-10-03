using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    private const float CO_TIME = 0.1f;

    [SerializeField]
    private GameObject _onWeapon;
    //private 플레이어 상태변환제어 변수
    [SerializeField]
    private PlayerStats[] _playerStats;
    private PlayerStatsHandler _controller;
    private GameObject[] _bullet;
    private float _buffTime = 10.0f;

    private void Start()
    {
        _controller = GetComponent<PlayerStatsHandler>();
        _playerStats[0] = _controller.CurrentStats;
    }

    public void OnGuied()
    {
        //gun 발사방식 타겟 찾아 n발 날라가기
    }

    public void OnDamageIncrease()
    {
        //n초간 player damage 증가
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
        //n초간 무적
        StartCoroutine(COOnInvincibility());
    }

    IEnumerator COOnInvincibility()
    {
        _controller.AddStatModifier(_playerStats[1]);
        while(_playerStats[1].invincibilityTime > 0)
        {
            _playerStats[1].invincibilityTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }
        _controller.AddStatModifier(_playerStats[0]);
    }

    IEnumerator COOnIncreaseDamage()
    {
        _controller.AddStatModifier(_playerStats[2]);
        while (_buffTime > 0)
        {
            _buffTime -= CO_TIME;
            yield return new WaitForSeconds(CO_TIME);
        }
        _controller.AddStatModifier(_playerStats[0]);
    }
}
