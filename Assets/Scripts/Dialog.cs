using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject text;
    private string message_ = " ";
    public GameObject button;
    public void DisplayText()
    {
        text.GetComponent<Text>().text = message_;
    }
    public void SetDialogMessage(string message)
    {
        message_ = message;
        DisplayText();
        

    }
    

}
