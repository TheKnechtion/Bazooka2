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

    [Header("Mortar Projectile")]
    [SerializeField] private GameObject AmmoPrefab;

    [Header("Laser Attributes")]
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float startingWidth;
    [SerializeField] private float endingWidth;
    private LineRenderer laserRenderer;

    private Vector3 targetPos;
    private bool enemySpawned;
    private bool targetingActive;

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

        targetingActive = false;
        timeUntilShoot = 0.0f;

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

    private IEnumerator TrackAndShoot()
    {
        targetingActive = true;

        ToggleLaser(true);

        while (timeUntilShoot < shootDelay)
        {
            timeUntilShoot += Time.deltaTime;
            SetLaserWidth(startingWidth, endingWidth);

            yield return null;
        }

        ShootMortar();

        ToggleLaser(false);

        timeUntilShoot = 0.0f;
        targetingActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            if (!targetingActive)
            {
                StartCoroutine(TrackAndShoot());
            }

            targetPos.x = other.transform.position.x;   
            targetPos.z = other.transform.position.z;

            SetLaserPos(targetPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            
        }
    }
    private void ShootMortar()
    {
        Instantiate(AmmoPrefab, targetPos, Quaternion.Euler(90, 0, 0));
    }
    private void EnableEnemy()
    {
        Instantiate(heldEnemy, mountPoint.position, Quaternion.identity);
    }

    private void ToggleLaser(bool active)
    {
        if (laserObject)
        {
            laserObject.SetActive(active);
        }
    }

    private void SetLaserPos(Vector3 pos)
    {
        if (laserObject)
        {
            laserObject.transform.position = pos;
        }
    }

    private void SetLaserWidth(float start, float end)
    {
        if (laserRenderer)
        {
            laserRenderer.widthMultiplier = Mathf.Lerp(start, end, timeUntilShoot / shootDelay);
        }
    }
}
