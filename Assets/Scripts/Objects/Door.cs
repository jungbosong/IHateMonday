using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Door�� ���̺곪 �濡 ���ӵǾ��־���� Why? ������Ȳ�̴� -> ������ٴ� ��°��� �Ǿ���ϴϱ�..
    public bool isLock = false;
    protected bool _isLocked = false;
    protected bool _isInBattle = false;
    //�κ��丮�� Ű�� �ִٴ� �����Ͽ� �۾�
    private Inventory _inventory;

    protected LayerMask _playerLayerMask;

    protected UI_Interaction _go = null;

    protected Animator _animator;
    protected Room _nearRoom;

    //protected SpriteRenderer _minimapRenderer;

    [Header("DoorDefaultSetting")]
    [SerializeField] protected BoxCollider2D _doorCollider;

    protected virtual void Awake()
    {

        //_inventory = Managers.Game.player.GetComponent<Inventory>();
        _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        _playerLayerMask = LayerMask.GetMask("Player");
        _animator = GetComponent<Animator>();

        SetNearRoom(transform.parent.GetComponent<Room>());
        _nearRoom.OnBattleStart += BattleStart;
        _nearRoom.OnBattleEnd += BattleEnd;

        //_minimapRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();
        //_minimapRenderer.color = Color.blue;

        if (isLock)
            Lock();
    }
    
    public void SetNearRoom(Room nearRoom)
    {
        _nearRoom = nearRoom;
    }
    public void BattleStart()
    {
        if(!_isInBattle)
        {
            _isInBattle = true;
            _doorCollider.enabled = true;
            _animator.Play("Close" , -1 , 0);
            //_minimapRenderer.color = Color.red;
            //_animator.Play("BattleStart" , -1 , 0);
        }
    }
    public void BattleEnd()
    {
        if (_isInBattle)
        {
            _isInBattle = false;
            //_animator.Play("BattleEnd", -1 , 0);
            //if(_isLocked)
            //    _minimapRenderer.color = Color.red;
            //else
            //    _minimapRenderer.color = Color.blue;
        }
    }
    public void Lock()
    {
        if(!_isLocked)
        {
            _isLocked = true;
            transform.GetChild(0).gameObject.SetActive(true);
            //_minimapRenderer.color = Color.red;
        }
    }
    public bool UnLock()
    {
        if(_inventory.UseKey())
        {
            return true;
            //_minimapRenderer.color = Color.blue;
        }

        return false;
    }

    public void OpenDoor()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        _isLocked = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLocked || _go || _isInBattle)
            return;

        if (0 != ( _playerLayerMask.value & ( 1 << collision.gameObject.layer ) ))
        {
            _go = Managers.UI.ShowPopupUI<UI_Interaction>();
            _go.Refresh(transform.position);
            _go.AddDelegate(UnLock);
            _go.OnEndInteraction += OpenDoor;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        Managers.Resource.Destroy(_go);

        _go = null;
    }
}