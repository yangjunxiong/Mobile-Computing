using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {
    public Text CountDown;
    public float time = 59.0f;
    public float timeLeft;
    public Image card;
    public Button searchForCards;
    public bool Switch = false;

	// Use this for initialization
	void Start () {
        timeLeft = time;
        CountDown.text = "00:00";
    }
	
	// Update is called once per frame
	void Update () {
        if (!Switch)
        {
            searchForCards.interactable = true;
            return;
        }
        searchForCards.interactable = false;
        timeLeft -= UnityEngine.Time.deltaTime;

        if (timeLeft >= 10)
        {
            CountDown.text = "00:" + (int)timeLeft;
        }
        else
        {
            CountDown.text = "00:0" + (int)timeLeft;
        }
        if (timeLeft <= 0)
        {
            timeLeft = time;
            Switch = false;
        }
	}
    public void turnOnSwitch()
    {
        Switch = true;
    }
}
