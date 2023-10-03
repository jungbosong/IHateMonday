using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameUIScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.TestGameUIScene;
        return true;
    }
}
