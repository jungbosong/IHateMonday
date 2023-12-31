using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static ResourceManager s_resourceManager = new ResourceManager();
    private static UIManager s_uiManager = new UIManager();
    private static SceneManagerEx s_SceneManager = new SceneManagerEx();
    private static SoundManager s_soundManager = new SoundManager();
    private static MapManager s_mapManager = new MapManager();
    private static PoolManager s_poolManager = new PoolManager();
    private static GameManager s_gameManager = new GameManager();

    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static UIManager UI { get { Init(); return s_uiManager; } }
    public static SceneManagerEx Scene { get { Init(); return s_SceneManager; } }
    public static SoundManager Sound { get { Init(); return s_soundManager; } }
    public static MapManager Map { get { return s_mapManager; } }
    public static PoolManager Pool { get { Init(); return s_poolManager; } }
    public static GameManager Game { get { Init();return s_gameManager; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        //PlayerPrefs.DeleteAll();
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
                go = new GameObject { name = "@Managers" };

            s_instance = Utils.GetOrAddComponent<Managers>(go);

            DontDestroyOnLoad(go);

            s_resourceManager.Init();
            s_soundManager.Init();
            s_poolManager.Init();
            s_gameManager.Init();
            Application.targetFrameRate = 60;
        }
    }
}
