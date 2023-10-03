using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonDoor : Door
{

    [SerializeField] private SpriteRenderer _Leftdoor;
    [SerializeField] private SpriteRenderer _Rightdoor;
    [SerializeField] private GameObject _player;
    private SpriteRenderer _playerSprite;
    private int _playerSortingOrder;

    protected override void Awake()
    {
        base.Awake();

        //Test (프리펩 만지고있어서 나중에 빼고 프리펩에서 직접연결하는 방식으로 합시다)
        {
            _Leftdoor = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _Rightdoor = transform.GetChild(2).GetComponent<SpriteRenderer>();
        }

        _player = GameObject.FindWithTag("Player");
        _playerSprite = _player.GetComponentInChildren<SpriteRenderer>();
        _playerSortingOrder = _playerSprite.sortingOrder;
    }
    private void OnEnable()
    {
        _animator.Play("Close" , -1 , 0);
    }

    void Update()
    {
        if (( transform.position - _player.transform.position ).magnitude > 5)
            return;

        float playerBottiomPosY = _player.transform.position.y - _playerSprite.bounds.size.y * 0.5f + _playerSprite.transform.localPosition.y;
        if (_Leftdoor.transform.position.y < playerBottiomPosY)
        {
            _Leftdoor.sortingOrder = _playerSortingOrder + 1;
            _Rightdoor.sortingOrder = _playerSortingOrder + 1;
        }
        else
        {
            _Leftdoor.sortingOrder = _playerSortingOrder - 1;
            _Rightdoor.sortingOrder = _playerSortingOrder - 1;
        }
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
