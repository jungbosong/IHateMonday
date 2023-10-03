using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayingSounds : MonoBehaviour
{
    [SerializeField] private SoundSO soundSO;
    public SoundSO SoundSO { set { soundSO = value; } }

    private HealthSystem _healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDamage += PlayingDamage;
        _healthSystem.OnDeath += PlayingDead;

        if ( soundSO.starting != null)
        {
            //Managers.Sound.Play("LobbyBgm", Define.Sound.Bgm);
            Managers.Sound.Play(soundSO.starting, Define.Sound.Effect);
        }
    }

    void PlayingDamage()
    {
        if (soundSO.damaging != null)
        {
            int result = Random.Range(0, 10);

            if( result== 0)
            {
                Managers.Sound.Play(soundSO.damaging, Define.Sound.Effect);
            }
        }
    }

    void PlayingDead()
    {
        if(soundSO.dead != null && soundSO.isBoss)
        {
            Managers.Sound.Play(soundSO.victory, Define.Sound.Effect);
            Managers.Sound.Play(soundSO.dead, Define.Sound.Effect);
        }else if(soundSO.dead != null)
        {
            Managers.Sound.Play(soundSO.dead, Define.Sound.Effect);
        }
    }
    
}
