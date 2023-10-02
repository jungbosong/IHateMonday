using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position += transform.right.normalized * GetComponent<BoxCollider2D>().size.x * 0.5f;
    }

    private void Update()
    {
        if(_isGuided)
        {
            if (_target == null)
            {
                _target = GetNearObjectInAngle();
            }

            if(_target != null)//1.0.2초마다 타겟감지 두트윈회전 //2.그냥 단순회전
            {
                   Vector2 _targetVector = _target.transform.position - transform.position;
                    float dot = Vector3.Dot(transform.right.normalized , _targetVector.normalized);
                    float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (angle < _findMaxAngle)
                {
                    Vector3 cross = Vector3.Cross(transform.right.normalized , _targetVector).normalized;
                    float flipValue = 1;
                    if (transform.rotation.eulerAngles.y > 170f)
                        flipValue = -1;
                        // 외적 결과 값에 따라 각도 반영
                        if (cross.z < 0)
                    {
                            angle = transform.rotation.eulerAngles.z - Mathf.Min(10 , angle) * flipValue;
                    }
                    else
                    {
                        angle = transform.rotation.eulerAngles.z + Mathf.Min(10 , angle) * flipValue;
                    }

                    transform.rotation = Quaternion.Lerp(transform.rotation , Quaternion.Euler(0 , transform.rotation.eulerAngles.y , angle) , 0.4f);
                    // angle이 윗 방향과 target의 각도.
                }
                else
                {
                    _target = null;
                }
            }
        }
    }
    private void LateUpdate()
    {
        if (_bulletDistance < _nowMoveDistance)
        {
            Managers.Resource.Destroy(this);
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.right.normalized * _bulletSpeed;
        _nowMoveDistance += (_rigidbody.velocity * Time.fixedDeltaTime).magnitude;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(0 != ( _wallCollisionLayer.value & (1 << collision.gameObject.layer)))
        {
            if (_deadSpawnAnimatorController != null)
            {
                GameObject go = Managers.Resource.Instantiate("Effects/OneShotEffect");
                go.GetComponent<OneShotEffect>().Init(collision.ClosestPoint(transform.position) , _deadSpawnAnimatorController);
            }
            Managers.Resource.Destroy(this);
        }
        else if(0 != ( _targetCollisionLayer.value & ( 1 << collision.gameObject.layer ) ))
        {
            //IDamageAble.GetDamage(_damage);

            /*
            if (collision.TryGetComponent<TopDownMovement>(out TopDownMovement movement))
            {
                movement.ApplyKnockback(transform , _attackData.konckbackPower , _attackData.knockbackTime);
            }
            */
            Managers.Resource.Destroy(this);
        }
        else if(0 != (_envCollisionLayer.value & ( 1 << collision.gameObject.layer ) ))
        {
            //Env오브젝트를 받아와서 그에 맞는 효과 (책이 총에 맞으면 펑 터지더라고요)
            Managers.Resource.Destroy(this);
        }
    }
}
