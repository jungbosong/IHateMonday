using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private GameObject _target;           // 카메라가 따라갈 대상
    [SerializeField] private float _moveSpeed = 2f;             // 카메라가 따라갈 속도
    private Vector3 _targetPosition;     // 대상의 현재 위치

    private void Start()
    {
        _target = GameObject.FindWithTag("Player");
    }

    // 카메라가 플레이어를 따라감
    void Update()
    {
        if (_target.gameObject != null)
        {
            _targetPosition.Set(_target.transform.position.x, _target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
