using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;  //�����۵����� �Ҵ�
    [SerializeField]
    private PlayerStats _baseStat;
    private PlayerStats _changeStat;

    private void OnTriggerEnter2D(Collider2D other) //������ ȹ��
    {
        if(other.tag == "Player")
        {
            if(itemData.type == ItemType.Passive)   //�нú� ������
            {
                _changeStat = _baseStat;
                _changeStat.statsChangeType = StatsChangeType.Add;
                ////�÷��̾� ���� ��ȭ
                if (itemData.consumables[0].type == ConsumableType.Hp)
                {
                    _changeStat.currentHp = (int)itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.Shield)
                {
                    _changeStat.shieldCount = (int)itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.AttackPower)
                {
                    _changeStat.attackPowerCoefficient = itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.AttackSpeed)
                {
                    _changeStat.attackSpeedCoefiicient = itemData.consumables[1].value;
                }
                else if (itemData.consumables[0].type == ConsumableType.BulletAmount)
                {
                    GameObject curGun = GameObject.FindGameObjectWithTag("Gun");
                    Gun handGun = curGun.GetComponent<Gun>();
                    int amount = handGun.GetMaxAmmunition();
                    int curAmount = handGun.GetAmmunition();
                    handGun.AddAmmunition(amount - curAmount);
                }
                else if (itemData.consumables[0].type == ConsumableType.MoveSpeed)
                {
                    _changeStat.moveSpeedCoefficient = itemData.consumables[1].value;
                }

                PlayerStatsHandler _handler = other.GetComponent<PlayerStatsHandler>();
                _handler.AddStatModifier(_baseStat);
                Managers.Resource.Destroy(this);
                //�÷��̾� ���� ��ȭ
            }
            else
            {   // ��Ƽ�� ������
                if (itemData.maxStackAmount > itemData.stack)    //������ ����üũ
                {
                    Inventory.s_instance.AddItem(itemData);
                }
            }
        }
    }
}
