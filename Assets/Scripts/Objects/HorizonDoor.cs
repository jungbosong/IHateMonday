using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonDoor : Door
{
    private void OnEnable()
    {
        _animator.Play("Close" , -1 , 0);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_isInBattle || _isLocked)
            return;

        if (0 != ( _playerLayerMask.value & ( 1 << collision.gameObject.layer ) ))
        {
            if (collision.contacts[0].point.x < transform.position.x)
            {
                _animator.Play("OpenA" , -1 , 0);
            }
            else
            {
                _animator.Play("OpenB" , -1 , 0);
            }
            _doorCollider.enabled = false;
        }
    }
}
