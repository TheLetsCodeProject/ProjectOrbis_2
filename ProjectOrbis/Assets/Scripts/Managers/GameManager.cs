﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orbis.Timing;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {

    public TimeData lastTime;


    #region Properties
    [SerializeField]
    private LevelAsset m_LevelToLoad;
    public LevelAsset LevelToLoad { get { return m_LevelToLoad; } }

    [SerializeField]
    private string m_username = "anonymous";
    public string Username {
        get {
            return m_username;
        }
        set {
            m_username = value;
            SimpleSerializer.SaveString("Username", value);
        }
    }


    [SerializeField]
    private GameObject WinScreen;


    [SerializeField]
    private GameObject m_Player;
    public GameObject Player { get {
            m_Player.SetActive(true);
            return m_Player; } }
    public List<LevelAsset> levels;
    #endregion

    #region Members
    public Timer LevelTimer = new Timer();


    #endregion

    public static GameManager ins;

    private void Awake()
    {
        if (ins == null) {
            ins = this;
        } else {
            Destroy(this.gameObject);
            Debug.Log("Deleted existing GameManager");

        }

        DontDestroyOnLoad(this.gameObject);
        Debug.Log(SimpleSerializer.IsFirstLoad());

        if (Environment.GetFlag("--demo")) {

            List<LevelAsset> OLDdemoLevels = LevelFactory.ConstructFromFolderCONFIG(Environment.GetPath("demo"));
            for (int i = 0; i < OLDdemoLevels.Count; i++) {
                levels.Insert(0, OLDdemoLevels[i]);
            }

            List<LevelAsset> demoLevels = LevelFactory.ConstructFromFolder(Environment.GetPath("demo"));
            for (int i = 0; i < demoLevels.Count; i++) {
                levels.Insert(0, demoLevels[i]);
            }
        }

        if (File.Exists(Environment.GetPath("save") + "/Username.txt")) {
            m_username = SimpleSerializer.LoadString("Username");
        }
        else m_username = "anonymous";
        
    }

    #region Logic

    public void StartGame()
    {
        Debug.Log("Map Started");
        LevelTimer.Start();

    }

    public void EndGame()
    {
        Debug.Log("Map Completed");
        if (LevelTimer.IsStarted) {
            LevelTimer.Stop();
            lastTime = LevelTimer.GetFormattedTime();

        }
        m_Player.SetActive(false);
        Instantiate(WinScreen);


    }
    public void LoadLevel(LevelAsset level)
    {
        if (level == null || level.LevelTexture == null) {
            Debug.LogError("Incomplete level asset: " + level.name);
            return;
        }

        m_LevelToLoad = level;
        SceneManager.LoadScene("LevelScene");


    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    #endregion

}
