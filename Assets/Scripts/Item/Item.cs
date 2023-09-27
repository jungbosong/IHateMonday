using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;  //아이템데이터 할당

    private void OnTriggerEnter2D(Collider2D other) //아이템 획득
    {
        if(other.tag == "Player")
        {
            if(itemData.type == ItemType.Passive)   //패시브 아이템
            {
                //플레이어 스탯 변화
            }
            else
            {   // 액티브 아이템
                if (itemData.maxStackAmount > itemData.stack)    //아이템 수량체크
                {
                    Inventory.s_instance.AddItem(itemData);
                }
            }
        }
    }
}
