using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoor : Door
{
    [Header ("Vertical Door Sort by Player Info")]
    [SerializeField] private SpriteRenderer _Leftdoor;
    [SerializeField] private SpriteRenderer _Rightdoor;
    [SerializeField] private GameObject _player;
    private SpriteRenderer _playerSprite;
    private int _playerSortingOrder;
    [SerializeField]
    float playerBottiomPosY;
    protected override void Awake()
    {
        base.Awake();

        _player = GameObject.FindWithTag("Player");
        _playerSprite = _player.GetComponentInChildren<SpriteRenderer>();
        _playerSortingOrder = _playerSprite.sortingOrder;
    }
    private void OnEnable()
    {
        Room nearRoom = null;
        float nearDistance = float.MaxValue;
        //일단 룸리스트 직접순회 하겠습니다..;
        foreach (Room room in Managers.Map.roomList)
        {
            float near = Mathf.Abs(room.center.y - transform.position.y) - room.width / 2f;

            if (near < nearDistance)
            {
                nearDistance = near;
                nearRoom = room;
            }
        }

        if (nearRoom is null)
            return;

        _animator.Play("Close" , -1 , 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInBattle)
            return;

        if (( transform.position - _player.transform.position ).magnitude > 15)
            return;

        playerBottiomPosY = _player.transform.position.y - _playerSprite.bounds.size.y * 0.5f+ _playerSprite.transform.localPosition.y;
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
            if (collision.contacts[0].point.y < transform.position.y)
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
