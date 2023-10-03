using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SoundSO", order = 1)]

public class SoundSO : ScriptableObject
{
    public string starting;
    public string damaging;
    public string dead;
    public string victory;
    public bool isBoss;
}
