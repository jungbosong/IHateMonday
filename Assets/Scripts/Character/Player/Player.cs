using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private Camera _mainCamera;

    private HealthSystem _healthSystem;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private GameObject _endPanel;
    private CharacterStatsHandler _characterStatsHandler;



    private void Awake()
    {
        _mainCamera = Camera.main;
        _characterStatsHandler = GetComponent<CharacterStatsHandler>();
        _healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        _healthSystem.OnDamage += OnDamage;
        _healthSystem.OnDeath += GameOver;

    }


    private void FixedUpdate()
    {
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void OnDamage()
    {
        //_characterStatsHandler.ChangeStat(-1f, 1);  //체력 1감소
        _playerRenderer.color = new Color32(200, 100, 100, 255);
        StartCoroutine(nameof(HitCo));
    }
    private IEnumerator HitCo()  //1초간 붉은색으로
    {
        yield return new WaitForSeconds(1f);
        _playerRenderer.color = Color.white;
    }

    

    void GameOver()
    {
        _endPanel.SetActive(true);
    }
}
