using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellfishBox : MonoBehaviour, IBox
{
    private ItemCreate create;
    private bool needKey = true;
    private int random;


    private void Awake()
    {
        create = GetComponent<ItemCreate>();
        random = Random.Range(0, 100);
        if (random < 30)    //키 필요 박스인지 설정
            needKey = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (needKey)
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
        create.OnCreateItem();
    }
}
