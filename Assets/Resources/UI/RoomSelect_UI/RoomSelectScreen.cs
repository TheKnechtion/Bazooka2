using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectScreen : MonoBehaviour
{
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform optionOne;
    [SerializeField] RectTransform optionTwo;
    [SerializeField] RectTransform proceedButton;
    [SerializeField] Material roomSelectScreen;

    int True = 1;
    int False = 0;

    Color darkGreen = new Color(.03f, 0.69f, 0.0f);
    Color lightGreen = new Color(.00f, 1.00f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isOptionOneSelected = false;
    bool isOptionTwoSelected = false;

    // Update is called once per frame
    void LateUpdate()
    {


        if (RectTransformUtility.RectangleContainsScreenPoint(optionOne, cursor.position))
        {
            roomSelectScreen.SetInt("_OptionOneSelected", 1);
            roomSelectScreen.SetColor("_OptionOneColor", lightGreen);
            isOptionOneSelected = true;
        }
        else
        {
            roomSelectScreen.SetInt("_OptionOneSelected", 0);
            roomSelectScreen.SetColor("_OptionOneColor", darkGreen);
            isOptionOneSelected = false;
        }


        if (RectTransformUtility.RectangleContainsScreenPoint(optionTwo, cursor.position))
        {
            roomSelectScreen.SetInt("_OptionTwoSelected", 1);
            roomSelectScreen.SetColor("_OptionTwoColor", lightGreen);
            isOptionTwoSelected = true;
        }
        else
        {
            roomSelectScreen.SetInt("_OptionTwoSelected", 0);
            roomSelectScreen.SetColor("_OptionTwoColor", darkGreen);
            isOptionTwoSelected = false;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(proceedButton, cursor.position))
        {
            roomSelectScreen.SetInt("_IsProceedSelected", 1);
        }
        else
        {
            roomSelectScreen.SetInt("_IsProceedSelected", 0);
        }

    }
}
