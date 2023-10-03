using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.StartScene;
        return true;
    }

    public void LoadGameScene()
    {
        Managers.Scene.ChangeScene(Define.Scene.DungeonScene);
    }
}
