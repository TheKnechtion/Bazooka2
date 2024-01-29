using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MortarBehavior : MonoBehaviour
{
    [Tooltip("The enemy you want to be using this mortar.")]
    [SerializeField] private GameObject heldEnemy;

    [Tooltip("The child transform where the enemy will spawn.")]
    [SerializeField] private Transform mountPoint;

    [SerializeField] private LayerMask playerMask;

    [Header("Mortar Attributes")]
    [Tooltip("How the close the player has to be for enemy to spawn in.")]
    [SerializeField] private float radius;
    [Tooltip("How long before mortar shoots after targeting.")]
    [SerializeField] private float shootDelay;
    [SerializeField]private float timeUntilShoot;

    [Header("Laser Attributes")]
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float startingWidth;
    [SerializeField] private float endingWidth;
    private LineRenderer laserRenderer;

    private Vector3 targetPos;
    private bool enemySpawned;

    private void Awake()
    {
        try
        {
            laserRenderer = laserObject.GetComponent<LineRenderer>();
        }
        catch
        {
            Debug.LogWarning("! Mortar line render not set !");
        }
    }
    void Start()
    {
        laserObject.SetActive(false);
        enemySpawned = false;

        timeUntilShoot = shootDelay;

        targetPos = new Vector3(0, 15, 0);
    }

    void Update()
    {
        if (!enemySpawned) 
        {
            if (Physics.CheckSphere(gameObject.transform.position, radius, playerMask))
            {
                //EnableEnemy();
                //enemySpawned = true;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            if (!laserObject.activeInHierarchy)
            {
                laserObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            timeUntilShoot -= Time.deltaTime;
            if (timeUntilShoot <= 0)
            {

                timeUntilShoot = shootDelay;
            }
            
            laserRenderer.widthMultiplier = Mathf.Lerp(startingWidth, endingWidth, 1/timeUntilShoot);

            targetPos.x = other.transform.position.x;   
            targetPos.z = other.transform.position.z;
            laserObject.transform.position = targetPos;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            if (laserObject.activeInHierarchy)
            {
                timeUntilShoot = shootDelay;
                laserObject.SetActive(false);
            }
        }
    }

    private void EnableEnemy()
    {
        Instantiate(heldEnemy, mountPoint.position, Quaternion.identity);
    }
}
