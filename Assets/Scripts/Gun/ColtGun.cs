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
            GameObject go = Managers.Resource.Instantiate("Bullets/NormalBullet", _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);

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
        yield return new WaitForSeconds(_reloadDelay);
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
}
