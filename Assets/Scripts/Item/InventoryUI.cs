using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text quatityText;

    public Image weaponIcon;
    public TMP_Text bulletNumText;

    public void SetItemUI(ItemData slot)
    {
        itemIcon.sprite = slot.icon;
        quatityText.text = slot.stack.ToString();
    }

    public void SetWeaponUI(Sprite slot, int ammunition, int maxAmmunition)
    {
        weaponIcon.sprite = slot;
        bulletNumText.text = $"{ammunition}/{maxAmmunition}";
    }
}
