using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

    public GameObject Button;

	// Use this for initialization
	void Start () {
        Button.GetComponent<Button>().onClick.AddListener(buttonClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void buttonClicked()
    {
        gameObject.GetComponent<AudioSource>().Play();
        Utility.ShowPauseMenu();
    }
}
