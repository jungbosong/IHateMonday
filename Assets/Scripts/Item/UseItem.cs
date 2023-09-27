using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField]
    private GameObject _onWeapon;
    //private 플레이어 상태변환제어 변수
    private GameObject[] _bullet;

    public void OnGuied()
    {
        //gun 발사방식 타겟 찾아 n발 날라가기
    }

    public void OnDamageIncrease()
    {
        //n초간 player damage 증가
    }

    public void OnDestroyBullet()
    {
        _bullet = GameObject.FindGameObjectsWithTag("Bullet");
        
        foreach(GameObject thisBullet in _bullet)
        {
            Managers.Resource.Destroy(thisBullet);
        }
    }

    public void OnInvincibilite()
    {
        //n초간 무적
    }
}
