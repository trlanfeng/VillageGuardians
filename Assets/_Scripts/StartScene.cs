﻿using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        SceneManager.LoadScene("Game", false);
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("load", 1);
        SceneManager.LoadScene("Game", false);
    }
}
