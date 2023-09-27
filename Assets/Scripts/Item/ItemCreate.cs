using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] items; //아이템들
    [SerializeField]
    private GameObject[] weapons;   //무기들
    private int index;  //랜덤인덱스
    private int itemType;   // 아이템 or 무기 정할 랜덤수

    public void OnCreateItem()
    {
        itemType = Random.Range(0, 2);
        index = Random.Range(0, items.Length);

        switch (itemType)
        {
            case 0:
                Managers.Instantiate(items[index]);
                break;
            case 1:
                Managers.Instantiate(weapons[index]);
                break;
        }
    }
}
