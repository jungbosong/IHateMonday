using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDesignScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.MapDesignScene;
        Managers.UI.ShowSceneUI<UI_DungeonScene>();
        //Managers.Game.Init();
        //Managers.Game.Start();
        return true;
    }
}
