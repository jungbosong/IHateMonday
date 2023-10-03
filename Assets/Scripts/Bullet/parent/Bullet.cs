using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected bool _isPlayerBullet;
    [SerializeField]protected bool _isGuided;
    [SerializeField]protected float _damage;
    [SerializeField]protected float _bulletSpeed;
    [SerializeField]protected float _bulletDistance;
    [SerializeField]protected float _nowMoveDistance = 0;
    [SerializeField]protected float _knockBack;
    [SerializeField]protected LayerMask _wallCollisionLayer;
    [SerializeField]protected LayerMask _targetCollisionLayer;
    [SerializeField]protected LayerMask _envCollisionLayer;
    [SerializeField] protected RuntimeAnimatorController _deadSpawnAnimatorController;
    [SerializeField] protected float _findMaxAngle;
    protected GameObject _target;
    public void Init(float damage, float bulletSpeed, float bulletDistance, float knockBack, bool isPlayerBullet, bool isGuided)
    {
        _damage = damage;
        _bulletSpeed = bulletSpeed;
        _bulletDistance = bulletDistance;
        _nowMoveDistance = 0;
        _knockBack = knockBack;
        _wallCollisionLayer = LayerMask.GetMask("Wall");
        //_envCollisionLayer = LayerMask.GetMask("Env");
        BulletTargetSetting(isPlayerBullet);
        _isGuided = isGuided;
        _target = null;
    }

    public void BulletTargetSetting(bool isPlayerBullet)
    {
        _isPlayerBullet = isPlayerBullet;

        if (_isPlayerBullet)
            _targetCollisionLayer = LayerMask.GetMask("Enemy");
        else
            _targetCollisionLayer = LayerMask.GetMask("Player");
    }
    public GameObject GetNearObjectInAngle()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        float nearDis = float.MaxValue;
        GameObject nearObject = null;
        Vector3 position = transform.position;

        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<HealthSystem>(out HealthSystem hs))
            {
                if (hs.CurrentHealth == 0)
                    continue;
            }
            else
                continue;

            Vector3 targetPos = obj.transform.position;
            Vector2 targetVector = (Vector2)( targetPos - position );
            float dis = targetVector.magnitude;
            float dot = Vector2.Dot(transform.right.normalized , targetVector.normalized);
            float checkAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (nearDis > dis && checkAngle < _findMaxAngle)
            {
                nearObject = obj;
                nearDis = dis;
            }
        }

        return nearObject;
    }
    public GameObject GetNearObjectInAngle(Vector3 position, Vector3 dir)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        float nearDis = float.MaxValue;
        GameObject nearObject = null;

        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<HealthSystem>(out HealthSystem hs))
            {
                if (hs.CurrentHealth == 0)
                    continue;
            }
            else
                continue;

            Vector3 targetPos = obj.transform.position;
            Vector2 targetVector = (Vector2)( targetPos - position );
            float dis = targetVector.magnitude;
            float dot = Vector2.Dot(dir , targetVector.normalized);
            float checkAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (nearDis > dis && checkAngle < _findMaxAngle)
            {
                nearObject = obj;
                nearDis = dis;
            }
        }

        return nearObject;
    }
}
