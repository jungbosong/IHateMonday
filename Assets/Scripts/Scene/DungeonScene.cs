using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.DungeonScene;
        Managers.UI.ShowSceneUI<UI_DungeonScene>();
        return true;
    }
}
