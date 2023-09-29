using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Door는 웨이브나 방에 종속되어있어야함 Why? 전투상황이다 -> 방을잠근다 라는것이 되어야하니까..
    public bool isLocked = false;
    private bool isOpened = true;
    private bool _isSide = false;
    //인벤토리에 키가 있다는 전제하에 작업
    private Inventory _inventory;
    private LayerMask _playerLayerMask;

    UI_Interaction go;
    private Animator _animator;

    [Header("DoorDefaultSetting")]
    [SerializeField] private BoxCollider2D _doorCollider;
    [SerializeField] private SpriteRenderer _door1;
    [SerializeField] private SpriteRenderer _door2;
    [SerializeField] private GameObject _player;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private int _playerSortingOrder;

    private void Awake()
    {
        //if(isLocked)
        //_inventory = ??.GetComponent<Inventory>();
        _playerLayerMask = LayerMask.GetMask("Player");
        _animator = GetComponent<Animator>();


        //Test
        {
            _playerSortingOrder = 10;
        } 
    }
    private void OnEnable()
    {
        Room nearRoom;
        float nearDistance = float.MaxValue;
        //일단 룸리스트 직접순회 하겠습니다..;
        foreach(Room room in Managers.Map.roomList)
        {
            float near = Mathf.Abs(room.center.x - transform.position.x) - room.height / 2f;
            near = Mathf.Min(near, Mathf.Abs(room.center.y - transform.position.y) - room.width / 2f);

            if(near < nearDistance)
            {
                nearDistance = near;
                nearRoom = room;
            }
        }

        if (nearRoom = null)
            return;

        if(Mathf.Abs(nearRoom.center.x - transform.position.x) - nearRoom.height / 2f <
            Mathf.Abs(nearRoom.center.y - transform.position.y) - nearRoom.width / 2f)
        {
            //가로가 더 작다
            _isSide = true;
        }
        else
        {
            _isSide = false;
        }

        _animator.Play("Close" , -1 , 0);

        isOpened = false;
    }
    private void Update()
    {
        if (( transform.position - _player.transform.position ).magnitude > 15)
            return;

        float playerBottiomPosY = _player.transform.position.y - _playerSprite.bounds.size.y + _playerSprite.transform.localPosition.y;
        if(_door1.transform.position.y < playerBottiomPosY)
        {
            _door1.sortingOrder = _playerSortingOrder + 1;
        }
        else
        {
            _door1.sortingOrder = _playerSortingOrder - 1;
        }

        if (_door2.transform.position.y < playerBottiomPosY)
        {
            _door2.sortingOrder = _playerSortingOrder + 1;
        }
        else
        {
            _door2.sortingOrder = _playerSortingOrder - 1;
        }
    }
    public void Lock()
    {
        if(!isLocked)
        {
            isLocked = true;
            if(_isSide)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    public bool UnLock()
    {
        //if(_inventory.UseKey())
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isLocked = false;

            return true;
        }

        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked)
            return;

        if (0 != ( _playerLayerMask.value & ( 1 << collision.gameObject.layer ) ))
        {
            go = Managers.UI.ShowPopupUI<UI_Interaction>();
            go.Refresh(transform.position);
            go.AddDelegate(UnLock);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Managers.Resource.Destroy(go);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (0 != ( _playerLayerMask.value & ( 1 << collision.gameObject.layer ) ))
        {
            if(_isSide)
            {
                if (collision.contacts[0].point.x > transform.position.x)
                {
                    _animator.Play("OpenA");
                }
                else
                {
                    _animator.Play("OpenB");
                }
            }
            else
            {
                if (collision.contacts[0].point.y > transform.position.y)
                {
                    _animator.Play("OpenA");
                }
                else
                {
                    _animator.Play("OpenB");
                }

            }
            _doorCollider.enabled = false;
        }
    }
}
