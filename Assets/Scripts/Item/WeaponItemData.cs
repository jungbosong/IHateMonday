using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item" , menuName = "New Weapon")]
public class WeaponItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public Sprite icon;
    public string WeaponName;//이건 무기이름으로할지 프리펩으로할지 고민되네요
}

