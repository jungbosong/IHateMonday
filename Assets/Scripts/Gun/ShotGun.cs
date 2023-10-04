using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotGun : SemiAutoGun
{
    Coroutine _myCoroutine;
    public override IEnumerator COFire()
    {
        _animator.SetBool("Reload" , false);
        isReload = false;

        isManualFireReady = false;
        isAutoFireReady = false;

        for (int i = 0 ; i < _bulletCount ; ++i)
        {
            Vector3 position = _shotPoint.position + (Vector3)Random.insideUnitCircle * 0.1f;
            GameObject go = Managers.Resource.Instantiate("Bullets/MagnumBullet" , position , _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy , _accuracy) , Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(GetDamage(_damage) , _bulletSpeed , _bulletDistance , _knockBack , true , true);
        }
        _animator.Play("ShotGun_Fire" , -1 , 0f);
        Managers.Sound.Play("ShotgunShot");

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
        yield return null;
    }

    public void InputBullet()
    {
        _magazine = Mathf.Min(_magazine + 1, _maxMagazine);

        if(_magazine == _maxMagazine)
        {
            _animator.Play("ShotGun_Reload" , -1 , 5.2f / 6f);
            isReload = false;
            _animator.SetBool("Reload" , false);
        }
    }

    public override void OnKeyDown()
    {
        if (!isManualFireReady)
            return;

        if (_magazine == 0)
        {
            if(!isReload)
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
