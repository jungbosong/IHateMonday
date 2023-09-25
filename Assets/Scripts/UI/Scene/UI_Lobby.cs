using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby : UI_Scene
{
    #region Enums
    enum Texts
    {
        TitleText,
    }
    enum Buttons
    {
        StartButton,
    }
    // 게임 오브젝트 -> GameObjects
    // 이미지 -> Images
    #endregion

    void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        //BindImage(typeof(Images));
        //BindObject(typeof(GameObjects));
        #endregion

        GetText((int)Texts.TitleText).text = "월요일이 좋겠냐?";
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickedStartButton);

        return true;
    }

    void OnClickedStartButton()
    {
        //Managers.Sound.Play("ClickBtnEff"); 
        Managers.Scene.ChangeScene(Define.Scene.DungeonScene);
    }
}