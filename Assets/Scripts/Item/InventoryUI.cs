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

    public TMP_Text keyNumText;

    public void SetItemUI(ItemData slot)
    {
        itemIcon.sprite = slot.icon;
        quatityText.text = slot.stack.ToString();
    }

    public void SetWeaponUI(Sprite slot, int ammunition, int maxAmmunition)
    {
        weaponIcon.sprite = slot;
        if (ammunition != 9999)
        {
            bulletNumText.fontSize = 36;
            bulletNumText.text = $"{ammunition}/{maxAmmunition}";
        }
        else
        {
            bulletNumText.fontSize = 80;
            bulletNumText.text = "¡Ä";
        }
    }

    public void SetKeyNumUI(int keyNum)
    {
        keyNumText.text = keyNum.ToString();
    }
}
