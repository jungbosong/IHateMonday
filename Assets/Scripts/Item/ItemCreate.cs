using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _items; //아이템들
    [SerializeField]
    private GameObject[] _weapons;   //무기들
    private int _index;  //랜덤인덱스
    private int _itemType;   // 아이템 or 무기 정할 랜덤수

    public void OnCreateItem()
    {
        _itemType = Random.Range(0, 3);

        switch (_itemType)
        {
            case 0:
                _index = Random.Range(0, _weapons.Length);    // 한번 뽑은 무기는 제외
                Managers.Resource.Instantiate(_weapons[_index].name);
                break;
            default:
                _index = Random.Range(0, _items.Length);
                Managers.Resource.Instantiate(_items[_index].name);
                break;
        }
    }
}
