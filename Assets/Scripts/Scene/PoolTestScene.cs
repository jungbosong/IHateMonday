using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.PoolTestScene;
        return true;
    }
}
