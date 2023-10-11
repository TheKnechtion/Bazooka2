using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    enum CanvasState { WIN,LOSE,EVAC,NONE}


    CanvasState UI_state;

    [SerializeField] private GameObject ObjSpace;
    [SerializeField] private GameObject StatusSpace;
    private TextMeshProUGUI objRenderer;
    private TextMeshProUGUI statusRenderer;

    private string[] thingsToSay;
    
    float timerTime;
    bool timerStarted;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);        
    }

    private void Start()
    {
        objRenderer = ObjSpace.GetComponent<TextMeshProUGUI>();
        statusRenderer = StatusSpace.GetComponent<TextMeshProUGUI>();

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
                statusRenderer.text = thingsToSay[1];
                break;
            case CanvasState.LOSE:
                statusRenderer.text = thingsToSay[0];
                break;
            case CanvasState.EVAC:

                //We grab the Static GameManager timer and pass it to the canvases timer float;
                timerTime = GameManager.evacTimer.TimeLeft;     
                //textRenderer.text = timerTime.ToString();
                objRenderer.SetText("Evacuate the Mission Zone!\n{0.00}", timerTime);
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
