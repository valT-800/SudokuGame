using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBlock : MonoBehaviour
{
    public GameObject label;
    public GameObject text;
    private int streakes = 0;
    private DateTime time;

    private void DisplayStreakes()
    {
        label.GetComponent<Text>().text = "Streakes";
        text.GetComponent<Text>().text = streakes.ToString();
    }
    private void DisplayTime()
    {
        label.GetComponent<Text>().text = "Time";
        text.GetComponent<Text>().text = time.ToString("mm:ss tt");
    }
    public void SetStreakes(int streakes)
    {
        this.streakes = streakes;
        DisplayStreakes();
    }

    public void SetTime(DateTime time)
    {
        this.time = time;
        DisplayTime();
    }
}
