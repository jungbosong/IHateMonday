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
    //private Event Action _event;

    private InventoryUI _inventoryUI;
    private UseItem _useItem;
    [SerializeField]
    private List<Item> _itemsList;
    [SerializeField]
    private Transform _dropPosition;
    private ItemSlot _selectItem;
    private int _itemListIndex = 0;
    


    public static Inventory s_instance;

    private void Awake()
    {
        s_instance = this;
        _inventoryUI = GetComponent<InventoryUI>();
        _useItem = GetComponent<UseItem>();
    }

    private void Start()
    {
        
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData.stack < itemData.maxStackAmount)
        {
            itemData.stack++;
            return;
        }
        else
            return;
    }

    public void ThrowItem(ItemData item)
    {
        Managers.Resource.Instantiate(item.dropPrefab, _dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    public void OnUse()
    {
        if(_selectItem.item.stack != 0)
        {
            _selectItem.item.stack--;
            if (_selectItem.item.consumables[0].type == ConsumableType.BulletGuide)  //유도
            {
                _useItem.OnGuied();
            }
            else if(_selectItem.item.consumables[0].type == ConsumableType.IncreaseDamage)   //순간적 데미지증가
            {
                _useItem.OnDamageIncrease();
            }
            else if(_selectItem.item.consumables[0].type == ConsumableType.BulletDelete)     //공포탄 불릿삭제
            {
                _useItem.OnDestroyBullet();
            }
            else if(_selectItem.item.consumables[0].type == ConsumableType.Invincibility)    //순간적 무적
            {
                _useItem.OnInvincibilite();
            }

            return;
        }
        else
        {
            return;
        }
    } 

    public void UpdateInventoryUI()
    {
        //inventoryUI set 호출
        _inventoryUI.Set(_selectItem);
    }

    public void ChangeItem()
    {
        //아이템 체인지 키 눌렀을때
        //inventoryUI Set 호출
        //itemsList[itemListIndex] 사용 -> index 값이 listLength 이상 => 0으로
        _itemListIndex = (_itemListIndex + 1) % _itemsList.Count;
        _selectItem.item = _itemsList[_itemListIndex].itemData;
        _inventoryUI.Set(_selectItem);
    }
}
