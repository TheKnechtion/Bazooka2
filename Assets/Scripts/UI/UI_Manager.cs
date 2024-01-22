using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEditor.Search;

public class UI_Manager : MonoBehaviour
{
    enum CanvasState { WIN,LOSE,EVAC,NONE}

    Camera mainCam;
    Canvas canvas;

    CanvasState UI_state;

    [SerializeField] private GameObject ObjSpace;
    [SerializeField] private GameObject StatusSpace;
    [SerializeField] private GameObject HpSpace;
    [SerializeField] private GameObject CurrentWeaponSpace;
    [SerializeField] private GameObject ActiveProjectileSpace;
    [SerializeField] private GameObject TipSpace;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject ReplayButton;
    [SerializeField] private GameObject RoomSelectScreen;
    //[SerializeField] private GameObject HeartList;


    [SerializeField] private GameObject Activate;
    [SerializeField] private GameObject ActivateText;

    [SerializeField] private GameObject Objective;
    [SerializeField] private GameObject ObjectiveText;

    static GameObject Activate_Sample;
    static GameObject ActivateText_Sample;
    static GameObject Objective_Sample;
    static GameObject ObjectiveText_Sample;

    static GameObject RoomSelectScreenRef;

    private TextMeshProUGUI objRenderer;
    private TextMeshProUGUI statusRenderer;
    //private TextMeshProUGUI HpRenderer;
    //private TextMeshProUGUI CurrentWeaponRenderer;
    //private TextMeshProUGUI ActiveProjectileRenderer;
    private TextMeshProUGUI TipRenderer;
    
    public Material TurkeyMaterial;

    public Texture2D newTurkey;

    private string[] statusArray, objArray;
    
    float timerTime;
    bool timerStarted;
    int minuteCount, secondCount;
    string seconds;

    int currentPlayerHp;
    int maxPlayerHp;


    PlayerController _playerController;

    private static UI_Manager _instance;

    public static UI_Manager instance
    {
        get
        {
            return _instance;
        }
    }



    public static event EventHandler OnPlayerClick;


    private void OnEnable()
    {
        _playerController.PlayerActions.Enable();
    }

    private void OnDisable()
    {
        _playerController.PlayerActions.Disable();
    }


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        _playerController = new PlayerController();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Objective_Sample = Objective;
        ObjectiveText_Sample = ObjectiveText;
        Activate_Sample = Activate;
        ActivateText_Sample = ActivateText;

        RoomSelectScreenRef = RoomSelectScreen;
    }

    private void Start()
    {
        mainCam = Camera.main;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = mainCam;

        TurkeyMaterial.mainTexture = newTurkey;

        objRenderer = ObjSpace.GetComponent<TextMeshProUGUI>();
        statusRenderer = StatusSpace.GetComponent<TextMeshProUGUI>();
        //HpRenderer = HpSpace.GetComponent<TextMeshProUGUI>();
        //CurrentWeaponRenderer = CurrentWeaponSpace.GetComponent<TextMeshProUGUI>();
        //ActiveProjectileRenderer = ActiveProjectileSpace.GetComponent<TextMeshProUGUI>();

        //Subscribes UI_Manager to GameManager. We use events to 

        UI_state = CanvasState.NONE;

        GameManager.OnPlayerLose += GameManager_OnPlayerLose;
        GameManager.OnPlayerWin += GameManager_OnPlayerWin;
        GameManager.OnEvacStart += GameManager_OnEvacStart;

        PlayerInfo.OnPlayerHpChange += PlayerInfo_OnPlayerHpChange;
        PlayerManager.OnPlayerWeaponChange += PlayerManager_OnPlayerWeaponChange;
        EnemySpawnManager.OnEnemyDeath += EnemySpawnManager_OnEnemyDeath;

        PlayerManager.OnPlayerShoot += PlayerManager_OnPlayerProjectileAmountChange;
        PlayerManager.OnPlayerWeaponChange += PlayerManager_OnPlayerProjectileAmountChange;

        PlayerProjectile.OnExplosion += PlayerManager_OnPlayerProjectileAmountChange;

        



        Item.OnWeaponPickUp += Item_OnWeaponPickUp;

        populateTextArray();


        maxPlayerHp = PlayerInfo.instance.maximumHP;
        currentPlayerHp = maxPlayerHp;

        //CurrentWeaponRenderer.text = PlayerInfo.instance.ownedWeapons[0].weaponName;
        //HpRenderer.text = $"HEALTH: \n{currentPlayerHp}/{maxPlayerHp}";


        //Button_Push.OnPlayerInRange += Activate_Sample_InteractUI;
        //Button_Push.OnPlayerOutOfRange += DeActivate_Sample_InteractUI;

        gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().planeDistance = 0.5f;

    }





    bool atStart = true;

    private void Update()
    {



        objRenderer.text = $"Enemies Left: {EnemySpawnManager.enemyCount}";

        if (atStart)
        {
            //ActiveProjectileRenderer.text = $"Active Projectiles: {PlayerManager.activeProjectiles}/{PlayerManager.currentWeapon.maxProjectilesOnScreen}";
            atStart = false;
        }


        if(tutorialShowTime > 0.0f)
        {
            tutorialShowTime -= Time.deltaTime;
        }
        else
        {
            TipSpace.SetActive(false);
        }


        switch (UI_state)
        {
            case CanvasState.WIN:
                statusRenderer.text = statusArray[1];
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                QuitButton.SetActive(true);
                ReplayButton.SetActive(true);

                break;
            case CanvasState.LOSE:
                statusRenderer.text = statusArray[0];
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                QuitButton.SetActive(true);
                ReplayButton.SetActive(true);

                break;
            case CanvasState.EVAC:

                //We grab the Static GameManager timer and pass it to the canvases timer float;
                timerTime = GameManager.evacTimer.TimeLeft;

                minuteCount = (int)(timerTime / 60);
                secondCount = (int)(timerTime % 60);

                seconds = (secondCount > 10) ? secondCount.ToString():$"0{secondCount}";

                objRenderer.SetText($"Evacuate the Mission Zone!\n{minuteCount}:{seconds}");
                    
                break;

            case CanvasState.NONE:                
                statusRenderer.text = statusArray[2];
                QuitButton.SetActive(false);
                ReplayButton.SetActive(false);
                break;
            default:
               UI_state = CanvasState.NONE;
                break;
        }

        //Debug.Log("UI state "+ UI_state);

        
        _instance = this;
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


    

    private void Activate_Sample_InteractUI(object sender, System.EventArgs e)
    {
        Activate_Sample.SetActive(true);
    }



    private void DeActivate_Sample_InteractUI(object sender, System.EventArgs e)
    {
        Activate_Sample.SetActive(false);
    }

    public static void Show_InteractUI(string txt)
    {
        ActivateText_Sample.GetComponent<TextMeshProUGUI>().text = txt;
        Activate_Sample.SetActive(true);
    }
    public static void StopShow_InteractUI()
    {
        Activate_Sample.SetActive(false);
    }

    public static void Show_ObjectiveUI(string txt)
    {
        ObjectiveText_Sample.GetComponent<TextMeshProUGUI>().text = txt;
        Objective_Sample.SetActive(true);
    }
    public static void StopShow_ObjectiveUI()
    {
        Objective_Sample.SetActive(false);
    }


    public static void Show_RoomSelect()
    {
        RoomSelectScreenRef.SetActive(true);
    }
    public static void StopShow_RoomSelect()
    {
        RoomSelectScreenRef.SetActive(false);
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

    private void PlayerInfo_OnPlayerHpChange(object sender, System.EventArgs e)
    {
        currentPlayerHp = PlayerInfo.instance.currentHP;
        maxPlayerHp = PlayerInfo.instance.maximumHP;

        //HpRenderer.text = $"HEALTH";

        // \n{currentPlayerHp}/{maxPlayerHp}
    }

    private void PlayerManager_OnPlayerWeaponChange(object sender, System.EventArgs e)
    {
        //CurrentWeaponRenderer.text = PlayerManager.currentWeapon.weaponName;
    }

    private void PlayerManager_OnPlayerProjectileAmountChange(object sender, System.EventArgs e)
    {
        //ActiveProjectileRenderer.text = $"Active Projectiles: {PlayerManager.activeProjectiles}/{PlayerManager.currentWeapon.maxProjectilesOnScreen}";
    }


    private void EnemySpawnManager_OnEnemyDeath(object sender, System.EventArgs e)
    {
        objRenderer.text = $"Enemies Left: {EnemySpawnManager.enemyCount}";
    }

    float tutorialShowTime = 0.0f;
    bool hasNotShownTip = true;
    private void Item_OnWeaponPickUp(object sender, System.EventArgs e)
    {
        if(hasNotShownTip)
        {
            TipSpace.SetActive(true);
            Item.OnWeaponPickUp -= Item_OnWeaponPickUp;
            hasNotShownTip= false;
            tutorialShowTime = 5.0f;
        }
        
    }


}
