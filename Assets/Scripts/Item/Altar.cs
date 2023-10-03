using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    
    private PlayerStats _changeStats;
    [SerializeField]
    private GameObject _player;
    private PlayerStatsHandler _playerStatsHandler;
    private PlayerInputController _playerInputController;
    private int _goodOrBad;

    private void Start()
    {
        _playerStatsHandler = _player.GetComponent<PlayerStatsHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _goodOrBad = Random.Range(0, 2);
            switch (_goodOrBad)
            {
                case 0:
                    GoodDice();
                    Managers.Resource.Destroy(this);
                    break;

                case 1:
                    BadDice();
                    Managers.Resource.Destroy(this);
                    break;
            }
        }
    }

    private void GoodDice()
    {
        int goodCase = Random.Range(0, 3);

        switch (goodCase)
        {
            case 0:                             //최대 체력 증가
                _changeStats.currentMaxHp = 1;
                _playerStatsHandler.AddStatModifier(_changeStats);
                break;

            case 1:                             //아이템 상자 생성
                goodCase = Random.Range(0, 2);
                string itemBoxName = 0 == goodCase ? "PassiveItemBox" : "ActiveItemBox";
                Managers.Resource.Instantiate("itemBoxName");
                break;

            case 2:
                _playerInputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
                GameObject curGun = GameObject.FindGameObjectWithTag("Gun");
                Gun handGun = curGun.GetComponent<Gun>();
                handGun.ChargingAmmunition();
                break;
        }
    }

    private void BadDice()
    {
        int badCase = Random.Range(0, 3);
        GameObject curGun;

        switch (badCase)
        {
            case 0:                                                     //최대 체력 감소
                _changeStats.currentMaxHp = -1;
                _playerStatsHandler.AddStatModifier(_changeStats);
                break;

            case 1:                                                     //무기 파괴
                _playerInputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
                curGun = GameObject.FindGameObjectWithTag("Gun");       
                _playerInputController.UnEquipWeapon(curGun.GetComponent<Gun>());
                Managers.Resource.Destroy(curGun);
                break;

            case 2:                                                     //탄약 잃음
                float rnd = Random.Range(25, 101) / 100f;
                _playerInputController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
                curGun = GameObject.FindGameObjectWithTag("Gun");
                Gun handGun = curGun.GetComponent<Gun>();
                handGun.Lostmmunition(rnd);
                break;
        }
    }
}
