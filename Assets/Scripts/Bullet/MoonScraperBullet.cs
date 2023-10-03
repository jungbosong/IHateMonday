using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScraperBullet : Bullet
{
    private Transform _parentTransform;
    private LineRenderer _lineRenderer;
    private int tagetLayer;
    [SerializeField]private int _smooth;

    private float _damageDelay = 0.1f;
    private float _damageTime;
    private bool _damageAble = true;
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        tagetLayer = LayerMask.GetMask("Wall") | LayerMask.GetMask("Env") | LayerMask.GetMask("Enemy");
    }

    public void LaserInit(int lineSize, Transform parent)
    {
        _lineRenderer.positionCount = lineSize;
        if(_isGuided)
        {
            _lineRenderer.positionCount = ( _lineRenderer.positionCount - 1 ) * _smooth + 1;
            //50으로 둔 이유는 값을 뭐로넣을지 애매해서.. ㅇㅋ?
        }
        _parentTransform = parent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = _parentTransform.right;
        Vector2 pos = _parentTransform.position;
        float length = _bulletDistance;
        _lineRenderer.SetPosition(0, pos);
        _lineRenderer.startWidth = transform.localScale.x;
        _lineRenderer.endWidth = transform.localScale.x;
        //레이를 쏠겁니다 포지션카운트만큼
        if (!_isGuided)
        {
            for (int i = 1 ; i < _lineRenderer.positionCount ; ++i)
            {

                RaycastHit2D hit = Physics2D.Raycast(pos , dir , length , tagetLayer);
                if (hit)
                {
                    length -= hit.distance;
                    dir = Vector2.Reflect(dir , hit.normal);
                    pos = hit.point + dir * 0.3f;
                    _lineRenderer.SetPosition(i , hit.point);


                    AddDamage(hit);
                }
                else
                {
                    _lineRenderer.SetPosition(i , pos + dir * length);
                    length = 0;
                }

            }
        }
        else
        {
            for (int i = 1 ; i <= _lineRenderer.positionCount / _smooth ; ++i)
            {
                dir.Normalize();
                _target = GetNearObjectInAngle(pos , dir);
                BeziarCurve b = new BeziarCurve();
                b.InputPosition(pos);
                if (_target == null || ((Vector2)_target.transform.position - pos).magnitude > length)
                {
                    RaycastHit2D hit = Physics2D.Raycast(pos , dir , length , tagetLayer);
                    if (hit)
                    {
                        b.InputPosition(hit.point * 1.05f);
                    }
                    else
                    {
                        b.InputPosition(pos + dir * length);
                    }
                }
                else
                {
                    Vector3 targetPos = _target.transform.position;
                    //여기서 중간에수선의 발의 좌표를 찾아서 그 점을 찍어줘야함
                    //dot(타겟까지벡터.normalize, 내벡터.normalize) -> cosA;
                    //타겟까지벡터.magnitude * cosA -> 정사영한 길이
                    Vector2 targetVector = ( (Vector2)_target.transform.position - pos );

                    b.InputPosition(pos + dir * Vector2.Dot(dir , targetVector.normalized) * targetVector.magnitude);
                    b.InputPosition(_target.transform.position);
                }

                Vector2 beforePos = pos;
                Vector2 nextPos = new Vector2();
                bool objectHit = false;
                for (int j  = 0 ; j < _smooth ; ++j)
                {
                    int index = ( i - 1 ) * _smooth + j + 1;// -> 첫회전에 1~5 두번째에 6~10
                    if (!objectHit)
                    {
                        nextPos = b.GetBeziarPosition(1.0f / _smooth * ( j + 1 ));

                        dir = nextPos - beforePos;
                       
                        RaycastHit2D hit = Physics2D.Raycast(beforePos , dir.normalized , dir.magnitude , tagetLayer);
                        if (hit)//무조건 여기 들어오게 되어있음 안들어오면 아무것도 안맞았다는소리라서
                        {
                            dir = Vector2.Reflect(dir.normalized , hit.normal);
                            nextPos = hit.point;
                            pos = hit.point + dir.normalized * 0.1f;
                            objectHit = true;
                            AddDamage(hit);
                        }
                    }
                    _lineRenderer.SetPosition(index , nextPos);
                    length -= dir.magnitude;
                    beforePos = nextPos;
                }
            }
        }
    }

    void AddDamage(RaycastHit2D hit)
    {
        if (!_damageAble)
            return;
        if ((1 << hit.transform.gameObject.layer) != _targetCollisionLayer)
            return;

        if (hit.transform.TryGetComponent<CharacterMovement>(out CharacterMovement movement))
        {
            movement.ApplyKnockback(hit.point , _knockBack , 0.05f);
        }
        if (hit.transform.TryGetComponent<HealthSystem>(out HealthSystem health))
        {
            health.ChangeHealth((int)(-_damage * _damageDelay));
            Camera.main.GetComponent<ShakeCamera>().Shake(ShakeType.Attack);
        }
    }

    private void LateUpdate()
    {
        if(_damageAble)
        {
            _damageAble = false;
            _damageTime = Time.time + _damageDelay;
        }
        else if (Time.time > _damageTime)
        {
            _damageAble = true;
        }

    }
}
