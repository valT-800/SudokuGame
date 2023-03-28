using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : Selectable
{

    public GameObject number_text;
    private int number = 0;
    private bool disabled;
    private void DisplayText()
    {
        if (number <= 0)
        {
            number_text.GetComponent<Text>().text = " ";
            enabled = true;
        }   
        else
        {
            number_text.GetComponent<Text>().text = number.ToString();
            enabled = false;
        }
    }
    private void ChangeText()
    {
        if (number <= 0)
        {
            number_text.GetComponent<Text>().text = " ";
        }
        else number_text.GetComponent<Text>().text = number.ToString();
    }

    public void SetNumber(int number)
    {
         this.number = number;    
         DisplayText();        
    }
    public void ChangeNumber(int number)
    {
        this.number = number;
        ChangeText();
        
    }


}
