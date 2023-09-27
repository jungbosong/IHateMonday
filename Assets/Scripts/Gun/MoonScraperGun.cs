using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScraperGun : LaserGun
{
    [Header("BulletDiscription")]
    [SerializeField] private MoonScraperBullet _moonScraperBullet;
    [SerializeField] private int _lineSize = 3;

    
    public override IEnumerator COFire()
    {
        _animator.SetBool("Fire" , true);
        GameObject go = Managers.Resource.Instantiate("Bullets/MoonScraperBullet", _shotPoint.position, _shotPoint.rotation);
        _moonScraperBullet = go.GetOrAddComponent<MoonScraperBullet>();
        _moonScraperBullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);
        _moonScraperBullet.LaserInit(_lineSize, _shotPoint);
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

    private void Update()
    {
        OnKeyDown();
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

    public override void OnLook(Vector2 worldPos)
    {
        //플레이어의 트랜스폼을 받아온다 - 멤버변수로 두고 Awake에서 받아올예정
        //플레이어에는 왼손좌표랑 오른손좌표도 필요하다.
        Vector2 playerPosition = new Vector3(0, 0);
        Vector2 playerLeftHandPosition = new Vector2(-0.3f, 0);
        Vector2 playerRightHandPosition = new Vector2(0.3f, 0);

        Vector2 leftdir;
        Vector2 rightdir;

        if ((playerPosition - worldPos).magnitude < (playerPosition - playerRightHandPosition).magnitude)
            return;


        leftdir = (worldPos - (Vector2)playerLeftHandPosition).normalized;
        rightdir = (worldPos - (Vector2)playerRightHandPosition).normalized;

        float leftRotZ = Mathf.Atan2(leftdir.y, leftdir.x) * Mathf.Rad2Deg;
        float rightRotZ = Mathf.Atan2(rightdir.y, rightdir.x) * Mathf.Rad2Deg;

        if (isLeftHand && Mathf.Abs(leftRotZ) < 75 && Mathf.Abs(rightRotZ) < 105)
        {
            isLeftHand = false;
        }
        else if (!isLeftHand && Mathf.Abs(rightRotZ) > 105 && Mathf.Abs(leftRotZ) > 75)
        {
            isLeftHand = true;
        }

        float rotY = 0f;
        float rotZ = 0f;
        if (isLeftHand)
        {
            transform.position = playerLeftHandPosition;
            rotZ = 180f - Mathf.Atan2(leftdir.y, leftdir.x) * Mathf.Rad2Deg;
            rotY = 180f;
        }
        else
        {
            transform.position = playerRightHandPosition;
            rotZ = Mathf.Atan2(rightdir.y, rightdir.x) * Mathf.Rad2Deg;
            rotY = 0;
        }

        transform.rotation = Quaternion.Euler(0, rotY, rotZ);
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
