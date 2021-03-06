﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    List<LevelAsset> levels;
    public GameObject PanelPrefab;
    public GameObject ListParent;
    public Text placeholder;
    // Use this for initialization
    void Start()
    {
        //Get the levels from game manager
        levels = GameManager.ins.levels;

        //Create a new panel for each level
        for (int i = 0; i < levels.Count; i++) {
            GameObject currentPanel = Instantiate(PanelPrefab, ListParent.transform);
            currentPanel.GetComponent<Panel>().Level = levels[i]; //Sets the panels level
        }

        placeholder.text = GameManager.ins.Username;

    }

    public void UpdateName(string name)
    {
        GameManager.ins.Username = name;
    }
}
