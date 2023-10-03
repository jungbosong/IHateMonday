using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DungeonScene : UI_Scene
{
    #region Enums
    enum Texts
    {
        KeyNumText,
        CurBulletNumText,
    }
    enum GameObjects
    {
        Life1,
        Life2,
        Life3,
        Life4,
        Life5,
        Life6,
        Life7,
        Life8,
        Shield1,
        Shield2,
        Shield3,
    }
    enum Images
    {
        ActiveItemImage,
        CurWeaponImage,
    }
    #endregion

    private const string LIFE_TEXT = "Life";
    private const string SHIELD_TEXT = "Shield";
    private const string FILLED_IMAGE_PATH = "Sprites/UI/filled_krabby_patty";
    private const string EMPTY_IMAGE_PATH = "Sprites/UI/empty_krabby_patty";

    private const int MAX_LIFE_NUM = 8;
    private const int MAX_SHIELD_NUM = 3;

    private Sprite _filledHealthImage;
    private Sprite _emptyHealthImage;

    void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        #endregion

        _filledHealthImage = Managers.Resource.Load<Sprite>(FILLED_IMAGE_PATH);
        _emptyHealthImage = Managers.Resource.Load<Sprite>(EMPTY_IMAGE_PATH);

        //GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickedStartButton);

        return true;
    }

    public void UpdatePlayerStatUI(PlayerStats playerStats)
    {
        GetText((int)Texts.KeyNumText).text = "0";

        int currentHP = playerStats.currentHp;
        int curMaxHP = playerStats.currentMaxHp;
        
        // 보유 체력
        for (int i = 1; i <= currentHP; i++)
        {
            string targetObjectName = LIFE_TEXT + i;
            GameObjects targetEnum = (GameObjects)Enum.Parse(typeof(GameObjects), targetObjectName);
            GameObject obj = GetObject((int)targetEnum).gameObject;
            obj.SetActive(true);
            obj.GetComponent<Image>().sprite = _filledHealthImage;
        }

        // 현재 최대 체력 - 보유 체력 만큼 빈 sprite로 표시
        for (int i = currentHP + 1; i <= curMaxHP; i++)
        {
            string targetObjectName = LIFE_TEXT + i;
            GameObjects targetEnum = (GameObjects)Enum.Parse(typeof(GameObjects), targetObjectName);
            GameObject obj = GetObject((int)targetEnum).gameObject;
            obj.SetActive(true);
            obj.GetComponent<Image>().sprite = _emptyHealthImage;
        }

        // limit 체력 - 현재 최대 체력 만큼은 숨기기
        for (int i = curMaxHP + 1; i <= MAX_LIFE_NUM; i++)
        {
            string targetObjectName = LIFE_TEXT + i;
            GameObjects targetEnum = (GameObjects)Enum.Parse(typeof(GameObjects), targetObjectName);
            GetObject((int)targetEnum).gameObject.SetActive(false);
        }

        int curShield = playerStats.shieldCount;
        // 현재 실드 보유 개수 표시
        for (int i = 1; i <= curShield; i++)
        {
            string targetObjectName = SHIELD_TEXT + i;
            GameObjects targetEnum = (GameObjects)Enum.Parse(typeof(GameObjects), targetObjectName);
            GetObject((int)targetEnum).gameObject.SetActive(true);
        }

        for (int i = curShield + 1; i <= MAX_SHIELD_NUM; i++)
        {
            string targetObjectName = SHIELD_TEXT + i;
            GameObjects targetEnum = (GameObjects)Enum.Parse(typeof(GameObjects), targetObjectName);
            GetObject((int)targetEnum).gameObject.SetActive(false);
        }
    }
    //void OnClickedStartButton()
    //{
    //    //Managers.Sound.Play("ClickBtnEff"); 
    //    Managers.Scene.ChangeScene(Define.Scene.DungeonScene);
    //}
}
