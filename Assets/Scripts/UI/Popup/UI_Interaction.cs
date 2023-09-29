using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interaction : UI_Popup
{
    public delegate bool InteractionDelegate();
    // 함수 포인트를 넣기 위한 리스트
    private LinkedList<InteractionDelegate> _list = new LinkedList<InteractionDelegate>();
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Refresh(Vector3 position)
    {
        transform.position = position;
        _animator.Play("InteractionAnim" , -1 , 0);
    }
    public void AddDelegate(InteractionDelegate func)
    {
        _list.AddLast(func);
    }
    public void OnDestroy()
    {
        //TODO 컨트롤러의 OnInteraction(있다면) 에서 자기자신을 뺀다.
    }
    public void OnInteraction()
    {
        foreach(InteractionDelegate func in _list)
        {
            if(func())
            {
                _list.Remove(func);
            }
        }

        if (_list.Count == 0)
        {
            Managers.Resource.Destroy(this);
        }

        return;
    }
}
