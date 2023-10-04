
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSlot
{
    public ItemData item;
    public int quantify;
}

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUI;
    private InventoryUI _inventoryUIComponent;
    private UseItem _useItem;
    [SerializeField]
    private List<Item> _itemsList;
    [SerializeField]
    private Transform _dropPosition;
    private Item _selectItem;
    private int _itemListIndex = 0;
    private int _key = 0;

    private Gun _handGun;
    private Gun _subGun;
    private PlayerInputController _controller;
    private CharacterController _characterController;

    private Sprite _curGunSprite;

    public void AddKey()
    {
        _key++;
        Debug.Log(_key);
        _inventoryUIComponent.SetKeyNumUI(_key);
    }
    public bool UseKey()
    {
        if (_key <= 0)
            return false;
        _key--;
        _inventoryUIComponent.SetKeyNumUI(_key);
        return true;
    }

    public void EquipWeapon(WeaponItemData data)
    {
        if (_handGun == null)
        {
            GameObject go = Managers.Resource.Instantiate($"Guns/{data.WeaponName}");
            _handGun = go.GetComponent<Gun>();
        }
        else
        {
            _controller.UnEquipWeapon(_handGun);
            if (_subGun == null)
            {
                _subGun = _handGun;
            }
            else
            {
                Managers.Resource.Instantiate($"Items/Gun/{_handGun.name}Item" , _dropPosition.position);
                Managers.Resource.Destroy(_handGun);
            }

            GameObject go = Managers.Resource.Instantiate($"Guns/{data.WeaponName}");
            _handGun = go.GetComponent<Gun>();
        }

        _controller.EquipWeapon(_handGun);
        _curGunSprite = data.icon;
    }

    public void SwapWeapon()
    {
        if (_handGun == null || _subGun == null)
            return;

        _controller.UnEquipWeapon(_handGun);
        Gun temp = _handGun;
        _handGun = _subGun;
        _subGun = temp;
        _controller.EquipWeapon(_handGun);
    }

    public static Inventory s_instance;

    private void Awake()
    {
        s_instance = this;
        _useItem = GetComponent<UseItem>();
        _controller = GetComponent<PlayerInputController>();
        _characterController = GetComponent<CharacterController>();
        _inventoryUIComponent = _inventoryUI.GetComponent<InventoryUI>();
        _controller.OnChangeWeaponEvent += SwapWeapon;

        GameObject go = Managers.Resource.Instantiate($"Guns/MagnumGun");
        _handGun = go.GetComponent<Gun>();
        _controller.EquipWeapon(_handGun);
        _curGunSprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
        _inventoryUIComponent.SetWeaponUI(_curGunSprite, _handGun.GetAmmunition(), _handGun.GetMaxAmmunition());
    }

    private void Start()
    {
        _characterController.OnChangeActiveEvent += ChangeItem;
        _characterController.OnUseActiveEvent += OnUse;

        foreach (Item item in _itemsList)
        {
            item.itemData.stack = 0;
        }
        _selectItem = _itemsList[_itemListIndex];
        _inventoryUIComponent.SetItemUI(_selectItem.itemData);
        _inventoryUIComponent.SetKeyNumUI(_key);
    }

    private void Update()
    {
        if (_handGun != null )
        {
            _inventoryUIComponent.SetWeaponUI(_curGunSprite, _handGun.GetAmmunition(), _handGun.GetMaxAmmunition());
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData.stack < itemData.maxStackAmount)
        {
            itemData.stack++;
            UpdateInventoryItemUI();
            return;
        }
        else
            return;
    }

    //public void ThrowItem(ItemData item)
    //{
    //    Managers.Resource.Instantiate(item.dropPrefab, _dropPosition.position, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));
    //}

    public void OnUse()
    {
        ItemData curItem = _itemsList[_itemListIndex].itemData;
        if (curItem.stack != 0)
        {
            curItem.stack--;
            if (curItem.consumables[0].type == ConsumableType.BulletGuide)  //����
            {
                _useItem.OnGuied();
            }
            else if (curItem.consumables[0].type == ConsumableType.IncreaseDamage)   //������ ����������
            {
                _useItem.OnDamageIncrease();
            }
            else if (curItem.consumables[0].type == ConsumableType.BulletDelete)     //����ź �Ҹ�����
            {
                _useItem.OnDestroyBullet();
            }
            else if (curItem.consumables[0].type == ConsumableType.Invincibility)    //������ ����
            {
                _useItem.OnInvincibilite();
            }
            UpdateInventoryItemUI();
            return;
        }
        else
        {
            UpdateInventoryItemUI();
            return;
        }
    }

    public void UpdateInventoryItemUI()
    {
        //inventoryUI set ȣ��
        _inventoryUIComponent.SetItemUI(_selectItem.itemData);
    }

    public void ChangeItem()
    {
        //������ ü���� Ű ��������
        //itemsList[itemListIndex] ���?-> index ���� listLength �̻� => 0����
        _itemListIndex = (_itemListIndex + 1) % _itemsList.Count;
        _selectItem = _itemsList[_itemListIndex];
        UpdateInventoryItemUI();
    }
}