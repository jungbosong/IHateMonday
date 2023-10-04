using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text quatityText;


    public void Set(ItemData slot)
    {
        icon.sprite = slot.icon;
        quatityText.text = slot.stack.ToString();
    }
}
