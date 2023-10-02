using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Active,
    Passive
}

public enum ConsumableType
{
    Hp,
    Shield,
    AttackPower,
    AttackSpeed,
    BulletAmount,
    MoveSpeed,
    BulletGuide,
    IncreaseDamage,
    BulletDelete,
    Invincibility
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    //public string toolTip;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public int stack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}