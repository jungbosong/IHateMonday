using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffect : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(Vector3 position, RuntimeAnimatorController controller)
    {
        transform.position = position;
        _animator.runtimeAnimatorController = controller;
    }
    // Update is called once per frame
    void Update()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Managers.Resource.Destroy(this);
        }
    }
}
