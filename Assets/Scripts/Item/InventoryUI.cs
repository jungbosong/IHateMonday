using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image icon;
    public Text quatityText;


    public void Set(ItemData slot)
    {
        icon.sprite = slot.icon;
        quatityText.text = slot.stack.ToString();
    }
}
