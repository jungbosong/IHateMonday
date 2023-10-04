using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackZone : Bullet
{
    private Rigidbody2D _rigidbody;
    private bool _hit = false;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        _nowMoveDistance += Time.deltaTime;
        if (_bulletDistance < _nowMoveDistance)
        {
            Managers.Resource.Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (0 != ( _targetCollisionLayer.value & ( 1 << collision.gameObject.layer ) ))
        {
            if (_deadSpawnAnimatorController != null)
            {
                GameObject go = Managers.Resource.Instantiate("Effects/OneShotEffect");
                go.GetComponent<OneShotEffect>().Init(collision.ClosestPoint(transform.position) , _deadSpawnAnimatorController);
            }


            if (collision.TryGetComponent<CharacterMovement>(out CharacterMovement movement))
            {
                movement.ApplyKnockback(collision.ClosestPoint(Managers.Game.player.transform.position) , _knockBack , 0.05f);
            }
            if (collision.TryGetComponent<HealthSystem>(out HealthSystem health))
            {
                health.ChangeHealth((int)-_damage);
            }

            Camera.main.GetComponent<ShakeCamera>().Shake(ShakeType.Attack);
        }
        else if (0 != ( _envCollisionLayer.value & ( 1 << collision.gameObject.layer ) ))
        {
            //Env오브젝트를 받아와서 그에 맞는 효과 (책이 총에 맞으면 펑 터지더라고요)
            if (_deadSpawnAnimatorController != null)
            {
                GameObject go = Managers.Resource.Instantiate("Effects/OneShotEffect");
                go.GetComponent<OneShotEffect>().Init(collision.ClosestPoint(transform.position) , _deadSpawnAnimatorController);
            }
        }
    }
}
