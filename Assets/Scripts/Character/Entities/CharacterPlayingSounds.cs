using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayingSounds : MonoBehaviour
{
    [SerializeField] private SoundSO soundSO;
    public SoundSO SoundSO { set { soundSO = value; } }

    private HealthSystem _healthSystem;


    private void Awake()
    {
    }
    void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem.OnDamage += PlayingDamage;
        _healthSystem.OnDeath += PlayingDead;

        if ( soundSO.starting != "")   //캐릭터 생성시 플레이
        {
            Managers.Sound.Play(soundSO.starting);
        }
    }

    void PlayingDamage()
    {
        if (soundSO.damaging != "" && soundSO.isBoss) //보스캐릭터가 피격될 경우
        {
            int result = Random.Range(0, 10);

            if( result== 0)    //랜덤 10프로의 확률로 피격 사운드 플레이
            {
                Managers.Sound.Play(soundSO.damaging, Define.Sound.Effect);
            }
        }else if(soundSO.damaging != null)  //징징이(플레이어)가 피격될 경우
        {
            Managers.Sound.Play(soundSO.damaging, Define.Sound.Effect);
        }
    }

    void PlayingDead()
    {
        if(soundSO.dead != "" && soundSO.isBoss)    //보스가 사망한 경우
        {
            Managers.Sound.Play(soundSO.dead, Define.Sound.Effect);  //보스 사망대사와 함께
            Managers.Sound.Play(soundSO.victory, Define.Sound.Effect);  //징징이 승리 대사 플레이
        }
        else if(soundSO.dead != "")
        {
            Managers.Sound.Play(soundSO.dead, Define.Sound.Effect);
        }
    }
    
}
