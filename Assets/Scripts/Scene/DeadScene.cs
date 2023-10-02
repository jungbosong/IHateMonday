using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadScene : BaseScene
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("DungeonScene");
    }
}
