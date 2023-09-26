using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool _isPlayerBullet;
    protected float _damage;
    protected float _bulletSpeed;
    protected float _bulletDistance;
    protected float _nowMoveDistance = 0;
    protected float _knockBack;
    protected LayerMask _wallCollisionLayer;
    protected LayerMask _targetCollisionLayer;
    protected LayerMask _envCollisionLayer;
    [SerializeField] protected GameObject _deadSpawnObject;
    public void Init(float damage, float bulletSpeed, float bulletDistance, float knockBack, bool isPlayerBullet)
    {
        _damage = damage;
        _bulletSpeed = bulletSpeed;
        _bulletDistance = bulletDistance;
        _knockBack = knockBack;
        _wallCollisionLayer = LayerMask.GetMask("Wall");
        //_envCollisionLayer = LayerMask.GetMask("Env");
        BulletTargetSetting(isPlayerBullet);
    }

    public void BulletTargetSetting(bool isPlayerBullet)
    {
        _isPlayerBullet = isPlayerBullet;

        if (_isPlayerBullet)
            _targetCollisionLayer = LayerMask.GetMask("Monster");
        else
            _targetCollisionLayer = LayerMask.GetMask("Player");
    }
}
