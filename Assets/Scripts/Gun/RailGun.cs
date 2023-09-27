using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RailGun : CharzingGun
{
    private LineRenderer[] _lineRenderers;

    private float[] _baseAngle = new float[4] { 90, 120, 150, 180 };

    private float[] _angle = new float[4];

    private int tagetLayer;

    protected override void Awake()
    {
        base.Awake();
        tagetLayer = LayerMask.GetMask("Wall") | LayerMask.GetMask("Env") | LayerMask.GetMask("Monster");
        _angle = (float[])_baseAngle.Clone();
        _lineRenderers = GetComponentsInChildren<LineRenderer>();

        for(int i = 0; i < 4; ++i)
        {
            _lineRenderers[i].positionCount = 3;
            _lineRenderers[i].enabled = false;
        }
        _lineRenderers[4].positionCount = 2;
        _lineRenderers[4].enabled = false;
    }

    public void Update()
    {
        if(_isShooting)
        {
            _shotCharzing += Time.deltaTime;

            //라인랜더링..레이캐스팅...
            for(int i = 0; i < 4; i++)
            {
                _angle[i] -= (_baseAngle[i] / _maxShotCharzing) * Time.deltaTime;

                if (_angle[i] < 0)
                {
                    _angle[i] = 0f;
                }
                if (_angle[i] > 90)
                {
                    continue;
                }
                else if (!_lineRenderers[i].enabled)
                    _lineRenderers[i].enabled = true;

                Vector2 dirRight = Quaternion.AngleAxis(_angle[i], Vector3.forward) * _shotPoint.right;
                dirRight.Normalize();
                RaycastHit2D hitRight = Physics2D.Raycast(_shotPoint.position, dirRight, 100f
                    /*, LayerMask.GetMask("Wall") | LayerMask.GetMask("Monster") | LayerMask.GetMask("Env")*/);
                Vector2 dirLeft = Quaternion.AngleAxis(-_angle[i], Vector3.forward) * _shotPoint.right;
                dirLeft.Normalize();
                RaycastHit2D hitLeft = Physics2D.Raycast(_shotPoint.position, dirLeft, 100f, tagetLayer);
                if (hitRight)
                    _lineRenderers[i].SetPosition(0, hitRight.point);
                else
                    _lineRenderers[i].SetPosition(0, (Vector2)_shotPoint.position + dirRight * 100f);
                _lineRenderers[i].SetPosition(1, _shotPoint.position);
                if (hitLeft)
                    _lineRenderers[i].SetPosition(2, hitLeft.point);
                else
                    _lineRenderers[i].SetPosition(2, (Vector2)_shotPoint.position + dirLeft * 100f);
            }

            if (!_lineRenderers[4].enabled)
                _lineRenderers[4].enabled = true;
            _lineRenderers[4].SetPosition(0, _shotPoint.position);
            RaycastHit2D hit = Physics2D.Raycast(_shotPoint.position, _shotPoint.right, 100f);
            if (hit)
                _lineRenderers[4].SetPosition(1, hit.point);
            else
                _lineRenderers[4].SetPosition(1, _shotPoint.position + _shotPoint.right * 100f);
        }
    }
    public override IEnumerator COFire()
    {
        yield return new WaitForSeconds(1);
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
        if (isReload || _magazine == 0)
            return;

        _isShooting = true;
        _shotCharzing = 0;
    }

    public override void OnKeyPress()
    {
    }

    public override void OnKeyUp()
    {
        if(_maxShotCharzing < _shotCharzing)
        {
            GameObject go = Managers.Resource.Instantiate("Bullets/NormalBullet", _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(_damage, _bulletSpeed, _bulletDistance, _knockBack, true);

            //Managers.Sound.Play("?");

            --_magazine;
            --_ammunition;
            _shotCharzing = 0;
            LineClear();
            _isShooting = false;
            StartCoroutine(COReload());
        }
    }

    public void LineClear()
    {
        foreach(LineRenderer lr in _lineRenderers)
        {
            lr.enabled = false;
        }
        _angle = (float[])_baseAngle.Clone();
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
        throw new System.NotImplementedException();
    }
}
