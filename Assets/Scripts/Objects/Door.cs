using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Door�� ���̺곪 �濡 ���ӵǾ��־���� Why? ������Ȳ�̴� -> ������ٴ� ��°��� �Ǿ���ϴϱ�..
    [SerializeField]protected bool _isLocked = false;
    protected bool _isInBattle = false;
    //�κ��丮�� Ű�� �ִٴ� �����Ͽ� �۾�
    private Inventory _inventory;

    protected LayerMask _playerLayerMask;

    protected UI_Interaction _go = null;

    protected Animator _animator;
    protected Room _nearRoom;

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
            //_animator.Play("BattleStart" , -1 , 0);
        }
    }
    public void BattleEnd()
    {
        if (_isInBattle)
        {
            _isInBattle = false;
            //_animator.Play("BattleEnd", -1 , 0);
        }
    }
    public void Lock()
    {
        if(!_isLocked)
        {
            _isLocked = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public bool UnLock()
    {
        if(_inventory.UseKey())
        {
            return true;
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