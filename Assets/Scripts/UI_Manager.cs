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
    [SerializeField] private GameObject HpSpace;
    [SerializeField] private GameObject CurrentWeaponSpace;
    [SerializeField] private GameObject ActiveProjectileSpace;
    [SerializeField] private GameObject TipSpace;
    [SerializeField] private GameObject TurkeyMeter;


    private TextMeshProUGUI objRenderer;
    private TextMeshProUGUI statusRenderer;
    private TextMeshProUGUI HpRenderer;
    private TextMeshProUGUI CurrentWeaponRenderer;
    private TextMeshProUGUI ActiveProjectileRenderer;
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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);        
    }

    private void Start()
    {
        TurkeyMaterial.mainTexture = newTurkey;

        objRenderer = ObjSpace.GetComponent<TextMeshProUGUI>();
        statusRenderer = StatusSpace.GetComponent<TextMeshProUGUI>();
        HpRenderer = HpSpace.GetComponent<TextMeshProUGUI>();
        CurrentWeaponRenderer = CurrentWeaponSpace.GetComponent<TextMeshProUGUI>();
        ActiveProjectileRenderer = ActiveProjectileSpace.GetComponent<TextMeshProUGUI>();

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

        CurrentWeaponRenderer.text = PlayerInfo.instance.ownedWeapons[0].weaponName;
        HpRenderer.text = $"HEALTH: \n{currentPlayerHp}/{maxPlayerHp}";




    }

    bool atStart = true;

    private void Update()
    {
        objRenderer.text = $"Enemies Left: {EnemySpawnManager.enemyCount}";

        if (atStart)
        {
            ActiveProjectileRenderer.text = $"Active Projectiles: {PlayerManager.activeProjectiles}/{PlayerManager.currentWeapon.maxProjectilesOnScreen}";
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
                break;
            case CanvasState.LOSE:
                statusRenderer.text = statusArray[0];
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
                break;
            default:
               UI_state = CanvasState.NONE;
                break;
        }

        //Debug.Log("UI state "+ UI_state);
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

    private void PlayerInfo_OnPlayerHpChange(object sender, System.EventArgs e)
    {
        currentPlayerHp = PlayerInfo.instance.currentHP;
        maxPlayerHp = PlayerInfo.instance.maximumHP;

        HpRenderer.text = $"HEALTH: \n{currentPlayerHp}/{maxPlayerHp}";
    }

    private void PlayerManager_OnPlayerWeaponChange(object sender, System.EventArgs e)
    {
        CurrentWeaponRenderer.text = PlayerManager.currentWeapon.weaponName;
    }

    private void PlayerManager_OnPlayerProjectileAmountChange(object sender, System.EventArgs e)
    {
        ActiveProjectileRenderer.text = $"Active Projectiles: {PlayerManager.activeProjectiles}/{PlayerManager.currentWeapon.maxProjectilesOnScreen}";
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
