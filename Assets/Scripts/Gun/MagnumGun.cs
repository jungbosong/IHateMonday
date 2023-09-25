using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnumGun : SemiAutoGun
{
    private void Update()
    {
        //Test¿ë
        OnKeyDown();
    }
    public override IEnumerator COFire()
    {
        isManualFireReady = false;
        isAutoFireReady = false;
        
        GameObject go = Managers.Resource.Instantiate("Bullets/NormalBullet", _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
        NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
        bullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);

        //Managers.Sound.Play("?");

        --_magazine;
        --_ammunition;

        yield return new WaitForSeconds(_manualFireDelay);
        isManualFireReady = true;
        yield return new WaitForSeconds(_autoFireDelay - _manualFireDelay);
        isAutoFireReady = true;
    }

    public override IEnumerator COReload()
    {
        isReload = true;
        yield return new WaitForSeconds(_reloadDelay);
        isReload = false;
        _magazine = Mathf.Min(_maxMagazine, _ammunition);
    }

    public override void OnKeyDown()
    {
        if (_ammunition == 0)
            return;

        if (isReload || !isManualFireReady)
            return;

        if(_magazine == 0)
        {
            StartCoroutine(COReload());
        }
        else if(isManualFireReady)
        {
            StopCoroutine(COFire());
            StartCoroutine(COFire());
        }
    }

    public override void OnKeyPress()
    {
        if (isReload || !isAutoFireReady || _magazine == 0)
            return;

        StopCoroutine(COFire());
        StartCoroutine(COFire());
    }

    public override void OnKeyUp()
    {
    }

    public override void OnRoll()
    {
    }
}
