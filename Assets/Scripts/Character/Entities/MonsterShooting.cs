using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterShooting : MonoBehaviour
{
    private CharacterController _controller;
    private ShootEnemyController _shootEnemyController;
    private MonsterAttackDataSO _monsterAttackData;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    //public AudioClip shootingClip;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _shootEnemyController = GetComponent<ShootEnemyController>();
        _monsterAttackData = _shootEnemyController.monsterAttackDataSO;
    }

    void Start()
    {
        _controller.OnAttackEvent += OnShoot;
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }

    private void OnShoot()
    {
        float projectilesAngleSpace = _monsterAttackData.multipleProjectilesAngle;
        int numberOfProjectilesPerShot = _monsterAttackData.numberOfProjectilesPerShot;

        // 캐릭터가 화살을 발사하는 모양이 부채꼴 모양이 되도록 각도를 미리 땡겨주는 것
        float minAngle = -(numberOfProjectilesPerShot / 2) * projectilesAngleSpace + 0.5f * _monsterAttackData.multipleProjectilesAngle;

        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-_monsterAttackData.spread, _monsterAttackData.spread);
            angle += randomSpread;
            CreateProjectile(_monsterAttackData, angle);
        }
    }

    private void CreateProjectile(MonsterAttackDataSO monsterAttackDataSO, float angle)
    {
        ShootBullet(
            projectileSpawnPosition.position,   // 발사 위치
            RotateVector2(_aimDirection, angle),    // 우리가 가진 angle로 벡터를 만들어냄
            monsterAttackDataSO    // 공격 정보
            );
    }

    public void ShootBullet(Vector2 startPosition, Vector2 direction, MonsterAttackDataSO monsterAttackDataSO)
    {
        //GameObject obj = objectPool.SpawnFromPool(monsterAttackDataSO.bulletNameTag);

        GameObject obj = Managers.Resource.Instantiate("MonsterAttacks/" + monsterAttackDataSO.projectilePrefab.name);

        obj.transform.position = startPosition;     // 시작 위치
        ShootingAttackController attackController = obj.GetComponent<ShootingAttackController>();
        attackController.InitializeAttack(direction, monsterAttackDataSO);

        obj.SetActive(true);
    }

    // degree만큼 회전한 벡터를 구함
    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
