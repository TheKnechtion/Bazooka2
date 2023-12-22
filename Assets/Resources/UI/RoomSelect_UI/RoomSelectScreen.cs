using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomSelectScreen : MonoBehaviour
{
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform optionOne;
    [SerializeField] RectTransform optionTwo;
    [SerializeField] RectTransform proceedButton;
    [SerializeField] Material roomSelectScreen;


    //reward - string
    
    //objective - string
    
    //sceneName - string


    int True = 1;
    int False = 0;

    PlayerController _playerController;

    Color darkGreen = new Color(.03f, 0.69f, 0.0f);
    Color lightGreen = new Color(.00f, 1.00f, 0.0f);

    // Start is called before the first frame update
    void Awake()
    {
        _playerController = new PlayerController();

        _playerController.PlayerAim.Select.performed += OnClick;
        _playerController.PlayerAim.Select.canceled -= OnClick;
    }

    bool isOptionOneSelected = false;
    bool isOptionTwoSelected = false;

    // Update is called once per frame
    void LateUpdate()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(optionOne, cursor.position) || isOptionOneSelected)
        {
            roomSelectScreen.SetInt("_OptionOneSelected", 1);
            roomSelectScreen.SetColor("_OptionOneColor", lightGreen);
        }
        else
        {
            roomSelectScreen.SetInt("_OptionOneSelected", 0);
            roomSelectScreen.SetColor("_OptionOneColor", darkGreen);
        }


        if (RectTransformUtility.RectangleContainsScreenPoint(optionTwo, cursor.position) || isOptionTwoSelected)
        {
            roomSelectScreen.SetInt("_OptionTwoSelected", 1);
            roomSelectScreen.SetColor("_OptionTwoColor", lightGreen);
        }
        else
        {
            roomSelectScreen.SetInt("_OptionTwoSelected", 0);
            roomSelectScreen.SetColor("_OptionTwoColor", darkGreen);
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

    void OnClick(InputAction.CallbackContext e)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(optionOne, cursor.position))
        {
            isOptionOneSelected = true;
        }
        else if (!RectTransformUtility.RectangleContainsScreenPoint(proceedButton, cursor.position))
        {
            isOptionOneSelected = false;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(optionTwo, cursor.position))
        {
            isOptionTwoSelected = true;
        }
        else if (!RectTransformUtility.RectangleContainsScreenPoint(proceedButton, cursor.position))
        {
            isOptionTwoSelected = false;
        }
    }


    private void OnEnable()
    {
        _playerController.PlayerAim.Enable();
    }

    private void OnDisable()
    {
        _playerController.PlayerAim.Disable();
    }

}
