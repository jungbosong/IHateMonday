using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordGun : CharzingGun
{
    private SpriteRenderer _renderer;
    protected override void Awake()
    {
        base.Awake();
        _renderer = transform.GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if(_isShooting)
        {
            _shotCharzing += Time.deltaTime;
            if(_shotCharzing > _maxShotCharzing)
            {
                _renderer.color = Color.red;
            }
        }
        else
        {
            _renderer.color = Color.white;
        }
    }
    public override IEnumerator COFire()
    {
        yield return null;
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
        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }
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
        if (!_isShooting)
            return;

        _animator.SetTrigger("Attack");
        
        if(GetSpeed(_maxShotCharzing) < _shotCharzing)
        {
            //강한공격
            GameObject go = Managers.Resource.Instantiate("Bullets/RailBullet", _shotPoint.position, _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy, _accuracy), Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(GetDamage(_maxCharzingValaue) , _bulletSpeed, _bulletDistance, _knockBack, true , true);

            _isShooting = false;
        }
        else
        {
            //일반공격
            GameObject go = Managers.Resource.Instantiate("Bullets/SwordAttackZone" , _shotPoint.position , _shotPoint.rotation * Quaternion.AngleAxis(Random.Range(-_accuracy , _accuracy) , Vector3.forward));
            NormalBullet bullet = go.GetOrAddComponent<NormalBullet>();
            bullet.Init(GetDamage(_minCharzingValaue) , _bulletSpeed , _bulletDistance , _knockBack , true , true);

            _isShooting = false;
        }
    }

    public override void OnRoll()
    {
        throw new System.NotImplementedException();
    }
}
