using UnityEngine;
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
        Application.LoadLevel("Game");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("load", 1);
        Application.LoadLevel("Game");
    }
}
