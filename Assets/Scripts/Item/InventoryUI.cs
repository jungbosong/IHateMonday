using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image icon;
    public Text quatityText;
    private ItemSlot _slot;


    public void Set(ItemSlot slot)
    {
        icon.sprite = slot.item.icon;
        quatityText.text = slot.item.stack.ToString();
    }
}
