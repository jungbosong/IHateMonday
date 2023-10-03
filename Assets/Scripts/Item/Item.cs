using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;  //아이템데이터 할당
    [SerializeField]
    private PlayerStats _baseStat;
    private PlayerStats _changeStat;

    private void OnTriggerEnter2D(Collider2D other) //아이템 획득
    {
        if(other.tag == "Player")
        {
            if(itemData.type == ItemType.Passive)   //패시브 아이템
            {
                _changeStat = _baseStat;
                _changeStat.statsChangeType = StatsChangeType.Add;
                ////플레이어 스탯 변화
                if (itemData.consumables[0].type == ConsumableType.Hp)
                {
                    _changeStat.currentHp = (int)itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.Shield)
                {
                    _changeStat.shieldCount = (int)itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.AttackPower)
                {
                    _changeStat.attackPowerCoefficient = itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.AttackSpeed)
                {
                    _changeStat.attackSpeedCoefiicient = itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.BulletAmount)
                {
                    GameObject curGun = GameObject.FindGameObjectWithTag("Gun");
                    Gun handGun = curGun.GetComponent<Gun>();
                    int amount = handGun.GetMAXAmmunition();
                    int curAmount = handGun.GetAmmunition();
                    handGun.AddAmmunition(amount - curAmount);
                }
                else if (itemData.consumables[0].type == ConsumableType.MoveSpeed)
                {
                    _changeStat.moveSpeedCoefficient = itemData.consumables[1].value;
                }

                PlayerStatsHandler _handler = other.GetComponent<PlayerStatsHandler>();
                _handler.AddStatModifier(_baseStat);
                Managers.Resource.Destroy(this);
            }
            else
            {   // 액티브 아이템
                if (itemData.maxStackAmount > itemData.stack)    //아이템 수량체크
                {                    
                    Inventory.s_instance.AddItem(itemData);
                }
                Managers.Resource.Destroy(this);
            }
        }
    }
}
