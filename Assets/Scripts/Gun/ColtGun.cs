using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColtGun : BurstGun
{
    public override IEnumerator COFire()
    {
        isAutoFireReady = false;
        _burstedBullet = 0;

        while (_burstedBullet != _maxBurstBullet && _magazine != 0)
        {
            GameObject go = Managers.Resource.Instantiate("Bullets/ColtBullet" , _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);


            _animator.Play("ColtGun_Fire" , -1 , 0f);
            //Managers.Sound.Play("?");

            --_magazine;
            --_ammunition;
            ++_burstedBullet;

            yield return new WaitForSeconds(_shootingDelay);
        }

        yield return new WaitForSeconds(_autoFireDelay);

        isAutoFireReady = true;
    }

    public override IEnumerator COReload()
    {
        isReload = true;
        _animator.SetBool("Reload" , true);
        yield return new WaitForSeconds(_reloadDelay);

        _animator.SetBool("Reload" , false);
        isReload = false;
        _magazine = Mathf.Min(_maxMagazine, _ammunition);
    }

    public override void OnKeyDown()
    {
        if (_ammunition == 0)
            return;

        if (isReload || !isAutoFireReady)
            return;

        if (_magazine == 0)
            StartCoroutine(COReload());
        else
            StartCoroutine(COFire());
    }

    public override void OnKeyPress()
    {
    }

    public override void OnKeyUp()
    {
    }
    public override void OnRoll()
    {
        StopCoroutine(COFire());
        isAutoFireReady = true;
        _burstedBullet = 0;
    }
}
