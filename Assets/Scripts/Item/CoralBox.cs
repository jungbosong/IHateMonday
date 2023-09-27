using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상자 생성
//위치값 받아서 생성

public class CoralBox : MonoBehaviour, IBox
{
    private ItemCreate _create;
    private bool _needKey = true;
    private int _random;


    private void Awake()
    {
        _create = GetComponent<ItemCreate>();
        _random = Random.Range(0, 100);
        if(_random <= 30)    //키 필요 박스인지 설정
            _needKey = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (_needKey)
                OnKeyUsePopUpUI();
            else
                OnKeyUse();
        }

    }

    public void OnKeyUsePopUpUI()
    {
        //키사용 박스 팝업 호출 , 게임정지
    }

    public void OnKeyUse()  //박스 오픈
    {
        Managers.Destroy(this);
        _create.OnCreateItem();
    }
}
