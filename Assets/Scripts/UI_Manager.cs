using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    enum CanvasState { WIN,LOSE,EVAC,NONE}


    CanvasState UI_state;

    [SerializeField] private GameObject textSpace;
    private TextMeshProUGUI textRenderer;

    private string[] thingsToSay;
    
    float timerTime;
    bool timerStarted;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);        
    }

    private void Start()
    {
        textRenderer = textSpace.GetComponent<TextMeshProUGUI>();


        //Subscribes UI_Manager to GameManager. We use events to 

        UI_state = CanvasState.EVAC;

        GameManager.OnPlayerLose += GameManager_OnPlayerLose;
        GameManager.OnPlayerWin += GameManager_OnPlayerWin;
        GameManager.OnEvacStart += GameManager_OnEvacStart;

        populateTextArray();

        timerStarted= false;
    }

    private void Update()
    {
        switch (UI_state)
        {
            case CanvasState.WIN:
                textRenderer.text = thingsToSay[1];
                break;
            case CanvasState.LOSE:
                textRenderer.text = thingsToSay[0];
                break;
            case CanvasState.EVAC:

                //We grab the Static GameManager timer and pass it to the canvases timer float;
                timerTime = GameManager.evacTimer.TimeLeft;     
                textRenderer.text = timerTime.ToString();
                break;

            case CanvasState.NONE:
                break;
            default:
               UI_state = CanvasState.NONE;
                break;
        }
    }

    private void populateTextArray()
    { 
        thingsToSay = new string[2];
        thingsToSay[0] = "You Lose";
        thingsToSay[1] = "You Win";
    }

    private void GameManager_OnEvacStart(object sender, System.EventArgs e)
    {
        UI_state= CanvasState.EVAC;
    }

    private void GameManager_OnPlayerWin(object sender, System.EventArgs e)
    {
        UI_state = CanvasState.WIN;
    }

    private void GameManager_OnPlayerLose(object sender, System.EventArgs e)
    {
        UI_state = CanvasState.LOSE;
    }
}
