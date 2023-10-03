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

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(_nearRoom.type == RoomType.Normal ||
            _nearRoom.type == RoomType.Wave||
            _nearRoom.type == RoomType.Boss)
        {
            float doorDistance = Mathf.Abs(_nearRoom.center.x - transform.position.x) ;
            float playerDistance = Mathf.Abs(_nearRoom.center.x - collision.transform.position.x);

            if (doorDistance > playerDistance)
            {
                Debug.Log($"{_nearRoom.center}");
                _nearRoom.OnBattleStart?.Invoke();
            }
        }
    }
}
