using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSquare : Selectable
{
    public GameObject number_text;
    private int number_ = 0;

    public void DisplayText()
    {
        number_text.GetComponent<Text>().text = number_.ToString();
    }

    public void SetNumber(int number)
    {
        number_ = number;
        DisplayText();
    }
}
