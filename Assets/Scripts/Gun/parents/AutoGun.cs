using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoGun : Gun
{
    //분류하기 편하기위해 그냥 넣었습니다
    protected override void Awake()
    {
        base.Awake();
        _gunType = GunType.Auto;
    }
}
