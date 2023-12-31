using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = .5f;

    private float _timeSinceLastChange = float.MaxValue;

    private CharacterStatsHandler _characterStatsHandler;
    private PlayerStatsHandler _playerStatsHandler;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    private GameObject _uiRoot;
    private UI_DungeonScene _uiDungeonScene;

    public int CurrentHealth { get; private set; }  
    public int MaxHealth => _characterStatsHandler.CurrentStats.currentMaxHp;

    private void Awake()
    {
        _characterStatsHandler = GetComponent<CharacterStatsHandler>();
        if (gameObject.CompareTag("Player"))
        {
            _playerStatsHandler = gameObject.GetComponent<PlayerStatsHandler>();
        }
    }

    void Start()
    {
        CurrentHealth = _characterStatsHandler.CurrentStats.currentHp;
    }

    void Update()
    {
        if (_timeSinceLastChange < healthChangeDelay)   
        {
            _timeSinceLastChange += Time.deltaTime;     
            if (_timeSinceLastChange >= healthChangeDelay)      
            {
                OnInvincibilityEnd?.Invoke();      
            }
        }
    }

    public bool ChangeHealth(int change)
    {
        if (change == 0 || _timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }        

        _timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        if (change > 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            //Test
            Camera.main.GetComponent<ShakeCamera>().Shake(ShakeType.Hit);
            OnDamage?.Invoke();
        }

        if (CurrentHealth <= 0)
        {
            CallDeath();
        }

        if (gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = new PlayerStats { statsChangeType = StatsChangeType.Override };
            playerStats.currentHp = CurrentHealth;
            _playerStatsHandler.AddStatModifier(playerStats);

            _uiRoot = Managers.UI.Root;
            _uiDungeonScene = _uiRoot.transform.Find("UI_DungeonScene").GetComponent<UI_DungeonScene>();
            _uiDungeonScene.UpdatePlayerStatUI();
        }

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}