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

    private string[] statusArray, objArray;
    
    float timerTime;
    bool timerStarted;
    int minuteCount, secondCount;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);        
    }

    private void Start()
    {
        objRenderer = ObjSpace.GetComponent<TextMeshProUGUI>();
        statusRenderer = StatusSpace.GetComponent<TextMeshProUGUI>();

        //Subscribes UI_Manager to GameManager. We use events to 

        UI_state = CanvasState.NONE;

        GameManager.OnPlayerLose += GameManager_OnPlayerLose;
        GameManager.OnPlayerWin += GameManager_OnPlayerWin;
        GameManager.OnEvacStart += GameManager_OnEvacStart;

        populateTextArray();
    }

    private void Update()
    {
        switch (UI_state)
        {
            case CanvasState.WIN:
                statusRenderer.text = statusArray[1];
                break;
            case CanvasState.LOSE:
                statusRenderer.text = statusArray[0];
                break;
            case CanvasState.EVAC:

                //We grab the Static GameManager timer and pass it to the canvases timer float;
                timerTime = GameManager.evacTimer.TimeLeft;

                minuteCount = (int)(timerTime / 60);
                secondCount = (int)(timerTime % 60);
                objRenderer.SetText($"Evacuate the Mission Zone!\n{minuteCount}:{secondCount}");
                    
                break;

            case CanvasState.NONE:
                objRenderer.text = statusArray[2];
                statusRenderer.text = statusArray[2];
                break;
            default:
               UI_state = CanvasState.NONE;
                break;
        }

        Debug.Log("UI state "+ UI_state);
    }

    private void populateTextArray()
    { 
        statusArray = new string[3];
        statusArray[0] = "You Lose";
        statusArray[1] = "You Win";
        statusArray[2] = "";

        

        objArray = new string[3];
        objArray[0] = "Defeat all enemies!";
        objArray[1] = $"Evacuate the Mission Zone!\n{minuteCount}:{secondCount}";
        objArray[2] = "";
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
