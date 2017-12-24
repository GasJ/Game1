using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject ResumeButton;
    public GameObject QuitButton;

	// Use this for initialization
	void Start () {
        ResumeButton.GetComponent<Button>().onClick.AddListener(() => 
        {
            Destroy(gameObject);
            GlobalController.IsShowingPauseMenu = false;
        });
        QuitButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Welcome");
            GlobalController.IsShowingPauseMenu = false;
        });

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
