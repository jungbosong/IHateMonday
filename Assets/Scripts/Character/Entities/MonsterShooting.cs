using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShooting : MonoBehaviour
{
    private CharacterController _controller;
    private ShootingAttackController _shootingAttackController;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public AudioClip shootingClip;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        //_controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    //private void OnShoot()
    //{
    //    float projectilesAngleSpace = rangedAttackData.multipleProjectilesAngle;
    //    int numberOfProjectilesPerShot = rangedAttackData.numberOfProjectilesPerShot;

    //    // 캐릭터가 화살을 발사하는 모양이 부채꼴 모양이 되도록 각도를 미리 땡겨주는 것
    //    float minAngle = -(numberOfProjectilesPerShot / 2) * projectilesAngleSpace + 0.5f * rangedAttackData.multipleProjectilesAngle;

    //    for (int i = 0; i < numberOfProjectilesPerShot; i++)
    //    {
    //        float angle = minAngle + projectilesAngleSpace * i;
    //        float randomSpread = Random.Range(-rangedAttackData.spread, rangedAttackData.spread);
    //        angle += randomSpread;
    //        CreateProjectile(rangedAttackData, angle);
    //    }
    //}

    //private void CreateProjectile(RangedAttackData rangedAttackData, float angle)
    //{
    //    _projectileManager.ShootBullet(
    //        projectileSpawnPosition.position,   // 발사 위치
    //        RotateVector2(_aimDirection, angle),    // 우리가 가진 angle로 벡터를 만들어냄
    //        rangedAttackData    // 공격 정보
    //        );

    //    if (shootingClip)
    //    {
    //        SoundManager.PlayClip(shootingClip);
    //    }
    //}

    //// 벡터를 사용해서 direction을 구해줘야 함
    //// 우리가 갖고 있는 것은 angle이므로 angle로 벡터를 만들어주는 것
    //// 뒤에서 곱한 벡터를 앞의 Quaternion으로 회전시킨 벡터가 나옴
    //private static Vector2 RotateVector2(Vector2 v, float degree)
    //{
    //    return Quaternion.Euler(0, 0, degree) * v;
    //}
}
