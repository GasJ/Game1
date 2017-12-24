using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeButtons : MonoBehaviour {

    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject exitButton;

    // Use this for initialization
    void Start () {
        startButton.GetComponent<Button>().onClick.AddListener(startButtonClicked);
        optionsButton.GetComponent<Button>().onClick.AddListener(optionsButtonClicked);
        exitButton.GetComponent<Button>().onClick.AddListener(exitButtonClicked);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void startButtonClicked()
    {
        SceneManager.LoadScene("Home");
    }

    void optionsButtonClicked()
    {

    }

    void exitButtonClicked()
    {
        Application.Quit();
    }
}
