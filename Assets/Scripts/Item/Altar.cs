using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField]
    private PlayerStats[] _stats;
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
            _goodOrBad = Random.RandomRange(0, 2);
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
            case 0:
                _playerStatsHandler.AddStatModifier(_stats[0]);
                break;

            case 1:
                Managers.Resource.Instantiate("");
                break;

            case 2:
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
            case 0:                                                     //ÃÖ´ë Ã¼·Â °¨¼Ò
                _playerStatsHandler.AddStatModifier(_stats[1]);
                break;

            case 1:
                curGun = GameObject.FindGameObjectWithTag("Gun");       //¹«±â ÆÄ±«
                _playerInputController.UnEquipWeapon(curGun.GetComponent<Gun>());
                Managers.Resource.Destroy(curGun);
                break;

            case 2:                                                     //Åº¾à ÀÒÀ½
                float rnd = Random.Range(25, 101) / 100f;
                curGun = GameObject.FindGameObjectWithTag("Gun");
                Gun handGun = curGun.GetComponent<Gun>();
                handGun.Lostmmunition(rnd);
                break;
        }
    }
}
