using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
