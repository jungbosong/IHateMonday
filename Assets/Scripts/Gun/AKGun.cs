using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKGun : AutoGun
{
    Animator animator;

    public override IEnumerator COFire()
    {
        isAutoFireReady = false;

        _animator.Play("AKGun_Fire" , -1 , 0f);
        
        GameObject go = Managers.Resource.Instantiate("Bullets/AKBullet", _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
        NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
        _player = GameObject.FindGameObjectWithTag("Player");
        isGuied = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsHandler>().CurrentStats.isGuied;
        if (buffBullet > 0 && isGuied)
        {
            bullet.Init(GetDamage(_damage), _bulletSpeed, _bulletDistance, _knockBack, true, isGuied);
            --buffBullet;
        }
        else
        {
            _player.GetComponent<UseItem>().OffGuied();
            bullet.Init(GetDamage(_damage), _bulletSpeed, _bulletDistance, _knockBack, true, isGuied);
        }


        Managers.Sound.Play("AKShot");

        --_magazine;
        --_ammunition;

        yield return new WaitForSeconds(GetSpeed(_autoFireDelay));

        isAutoFireReady = true;
    }

    public override IEnumerator COReload()
    {
        isReload = true;
        _animator.SetBool("Reload", true);
        
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

    }

    public override void OnKeyPress()
    {
        if (isReload || !isAutoFireReady || _magazine == 0)
            return;

        StartCoroutine(COFire());
    }

    public override void OnKeyUp()
    {
    }
    public override void OnRoll()
    {
    }
}
