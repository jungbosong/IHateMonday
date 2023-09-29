using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnumGun : SemiAutoGun
{
    public override IEnumerator COFire()
    {
        isManualFireReady = false;
        isAutoFireReady = false;

        GameObject go = Managers.Resource.Instantiate("Bullets/MagnumBullet" , _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
        NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
        bullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);

        _animator.Play("MagnumGun_Fire" , -1 , 0f);
        //Managers.Sound.Play("?");

        --_magazine;

        yield return new WaitForSeconds(_manualFireDelay);
        isManualFireReady = true;
        yield return new WaitForSeconds(_autoFireDelay - _manualFireDelay);
        isAutoFireReady = true;
    }

    public override IEnumerator COReload()
    {
        isReload = true;
        _animator.SetBool("Reload" , true);
        yield return new WaitForSeconds(_reloadDelay);

        _animator.SetBool("Reload" , false);
        isReload = false;
        _magazine = _maxMagazine;
    }

    public override void OnKeyDown()
    {
        if (_ammunition == 0)
            return;

        if (isReload || !isManualFireReady)
            return;

        if (_magazine == 0)
        {
            StartCoroutine(COReload());
        }
        else if (isManualFireReady)
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
