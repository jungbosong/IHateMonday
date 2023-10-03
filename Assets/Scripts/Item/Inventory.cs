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
    private InventoryUI _uiComponent;
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

    public void AddKey()
    {
        _key++;
    }
    public bool UseKey()
    {
        if (_key <= 0)
            return false;
        _key--;
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
            if (_subGun != null)
            {
                Managers.Resource.Instantiate($"Items/{_subGun.name}Item", _dropPosition.position);
                Managers.Resource.Destroy(_subGun);
            }

            _subGun = _handGun;
            GameObject go = Managers.Resource.Instantiate($"Guns/{data.WeaponName}");
            _handGun = go.GetComponent<Gun>();
        }

        _controller.EquipWeapon(_handGun);
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
        _uiComponent = _inventoryUI.GetComponent<InventoryUI>();
    }

    private void Start()
    {
        _characterController.OnChangeActiveEvent += ChangeItem;
        _characterController.OnUseActiveEvent += OnUse;
        
        foreach(Item item in _itemsList)
        {
            item.itemData.stack = 0;
        }
        _selectItem = _itemsList[_itemListIndex];
        _uiComponent.Set(_selectItem.itemData);
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData.stack < itemData.maxStackAmount)
        {
            itemData.stack++;
            UpdateInventoryUI();
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
            if (curItem.consumables[0].type == ConsumableType.BulletGuide)  //유도
            {
                _useItem.OnGuied();
            }
            else if (curItem.consumables[0].type == ConsumableType.IncreaseDamage)   //순간적 데미지증가
            {
                _useItem.OnDamageIncrease();
            }
            else if (curItem.consumables[0].type == ConsumableType.BulletDelete)     //공포탄 불릿삭제
            {
                _useItem.OnDestroyBullet();
            }
            else if (curItem.consumables[0].type == ConsumableType.Invincibility)    //순간적 무적
            {
                _useItem.OnInvincibilite();
            }
            UpdateInventoryUI();
            return;
        }
        else
        {
            UpdateInventoryUI();
            return;
        }
    }

    public void UpdateInventoryUI()
    {
        //inventoryUI set 호출
        _uiComponent.Set(_selectItem.itemData);
    }

    public void ChangeItem()
    {
        //아이템 체인지 키 눌렀을때
        //itemsList[itemListIndex] 사용 -> index 값이 listLength 이상 => 0으로
        _itemListIndex = (_itemListIndex + 1) % _itemsList.Count;
        _selectItem = _itemsList[_itemListIndex];
        UpdateInventoryUI();
    }

}
