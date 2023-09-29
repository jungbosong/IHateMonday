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

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public int CurrentHealth { get; private set; }  
    public int MaxHealth => _characterStatsHandler.CurrentStats.currentMaxHp;

    private void Awake()
    {
        _characterStatsHandler = GetComponent<CharacterStatsHandler>();
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

        Debug.Log("¸Â¾Ò´Ù! " + gameObject.name + ", CurrentHealth : " + CurrentHealth);

        if (change > 0)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
        }

        if (CurrentHealth <= 0f)
        {
            CallDeath();
        }

        return true;
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}