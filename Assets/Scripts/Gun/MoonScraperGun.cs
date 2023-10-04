using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScraperGun : LaserGun
{
    [Header("BulletDiscription")]
    [SerializeField] private MoonScraperBullet _moonScraperBullet;
    [SerializeField] private int _lineSize = 3;
    private bool _isGuied;
    private int _buffBullet = 0;

    public override IEnumerator COFire()
    {
        _animator.SetBool("Fire" , true);
        GameObject go = Managers.Resource.Instantiate("Bullets/MoonScraperBullet", _shotPoint.position, _shotPoint.rotation);
        _moonScraperBullet = go.GetOrAddComponent<MoonScraperBullet>();
        _isGuied = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsHandler>().CurrentStats.isGuied;
        if (_buffBullet < 10)
        {
            _moonScraperBullet.Init(GetDamage(_damage), _bulletSpeed, _bulletDistance, _knockBack, true, _isGuied);
            --_buffBullet;
        }
        else
        {
            _player.GetComponent<UseItem>().OffGuied();
            _moonScraperBullet.Init(GetDamage(_damage), _bulletSpeed, _bulletDistance, _knockBack, true, _isGuied);
        }
        _moonScraperBullet.LaserInit(_lineSize , _shotPoint);
        _isShooting = true;

        while (true)
        {
            if (!_isShooting)
            {
                _animator.SetBool("Fire" , false);
                break;
            }

            --_magazine;
            --_ammunition;

            yield return new WaitForSeconds(_autoFireDelay);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_isShooting)
        {
            if (_magazine == 0)
                _isShooting = false;
        }
        else if(_moonScraperBullet)
        {
            Managers.Resource.Destroy(_moonScraperBullet);
        }
    }

    public override void OnKeyDown()
    {
        if (_isShooting)
            return;
        if (isReload)
            return;
        if (_magazine == 0)
            return;
        StartCoroutine(COFire());
    }


    public override void OnKeyUp()
    {
        _animator.SetBool("Fire" , false);
        _isShooting = false;
    }

    public override void OnRoll()
    {
        _animator.SetBool("Fire" , false);
        _isShooting = false;
    }

    public override IEnumerator COReload()
    {
        throw new System.NotImplementedException();
    }
    public override void OnKeyPress()
    {
    }

}
