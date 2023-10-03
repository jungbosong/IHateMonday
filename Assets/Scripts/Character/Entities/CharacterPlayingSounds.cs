using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayingSounds : MonoBehaviour
{
    [SerializeField] private SoundSO soundSO;
    public SoundSO SoundSO { set { soundSO = value; } }

    private HealthSystem _healthSystem;

    void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem.OnDamage += PlayingDamage;
        _healthSystem.OnDeath += PlayingDead;

        if ( soundSO.starting != "" && soundSO.isPlayer)   //캐릭터 생성시
        {
            Managers.Sound.Play(soundSO.starting, Define.Sound.Bgm);  //배경음악 재생
        }else if( soundSO.starting != "")
        {
            Managers.Sound.Play(soundSO.starting);  //플레이어 외의 캐릭터 생성시 재생
        }
    }

    void PlayingDamage()
    {
        if (soundSO.damaging != "" && soundSO.isBoss) //보스캐릭터가 피격될 경우
        {
            int result = Random.Range(0, 10);

            if( result== 0)    //랜덤 10프로의 확률로 피격 사운드 플레이
            {
                Managers.Sound.Play(soundSO.damaging);
            }
        }else if(soundSO.damaging != "")  //징징이(플레이어)가 피격될 경우
        {
            Managers.Sound.Play(soundSO.damaging);
        }
    }

    void PlayingDead()
    {
        if(soundSO.dead != "" && soundSO.isBoss)    //보스가 사망한 경우
        {
            Managers.Sound.Play(soundSO.dead);  //보스 사망대사와 함께
            Managers.Sound.Play(soundSO.victory);  //징징이 승리 대사 플레이
        }
        else if(soundSO.dead != "")
        {
            Managers.Sound.Play(soundSO.dead);
        }
    }
    
}
