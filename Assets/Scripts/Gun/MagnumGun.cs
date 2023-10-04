using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnumGun : SemiAutoGun
{
    Coroutine _myCoroutine;
    public override IEnumerator COFire()
    {
        isManualFireReady = false;
        isAutoFireReady = false;

        for (int i = 0 ; i < _bulletCount ; ++i)
        {
            GameObject go = Managers.Resource.Instantiate("Bullets/MagnumBullet" , _shotPoint.position , _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy , _accuracy) , Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(GetDamage(_damage) , _bulletSpeed , _bulletDistance , _knockBack , true , true);
        }
        _animator.Play("MagnumGun_Fire" , -1 , 0f);
        Managers.Sound.Play("MagnumShot");

        --_magazine;

        yield return new WaitForSeconds(GetSpeed(_manualFireDelay));
        isManualFireReady = true;
        yield return new WaitForSeconds(GetSpeed(_autoFireDelay) - GetSpeed(_manualFireDelay));
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
        if (isReload || !isManualFireReady)
            return;

        if (_magazine == 0)
        {
            StartCoroutine(COReload());
        }
        else if (isManualFireReady)
        {
            if (_myCoroutine is not null)
                StopCoroutine(_myCoroutine);
            _myCoroutine = StartCoroutine(COFire());
        }
    }

    public override void OnKeyPress()
    {
        if (isReload || !isAutoFireReady || _magazine == 0)
            return;
        if(_myCoroutine is not null)
            StopCoroutine(_myCoroutine);
        _myCoroutine = StartCoroutine(COFire());
    }

    public override void OnKeyUp()
    {
    }

    public override void OnRoll()
    {
    }

}
