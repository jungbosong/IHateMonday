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
        itemType = Random.Range(0, 3);

        switch (itemType)
        {
            case 0:
                index = Random.Range(0, weapons.Length);    // 한번 뽑은 무기는 제외
                Managers.Resource.Instantiate(weapons[index]);
                break;
            default:
                index = Random.Range(0, items.Length);
                Managers.Resource.Instantiate(items[index]);
                break;
        }
    }
}
