using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MortarBehavior : MonoBehaviour, IParticleCaller
{
    [Tooltip("The enemy you want to be using this mortar.")]
    [SerializeField] private GameObject heldEnemy;
    private GameObject enemy;
    private EnemyBehavior enBehav;

    [Tooltip("The child transform where the enemy will spawn.")]
    [SerializeField] private Transform mountPoint;
    [SerializeField] private DestroyableObject MortarBody;


    [SerializeField] private LayerMask playerMask;

    [Header("Mortar Attributes")]
    [Tooltip("How the close the player has to be for enemy to spawn in.")]
    [SerializeField] private float radius;


    [Tooltip("How long before mortar shoots after targeting.")]
    [SerializeField] private float shootDelay;
    private float timeUntilShoot;

    [Tooltip("After shooting, how long to wait before targeting again.")]
    [SerializeField] private float targetingDelay;

    [Header("Mortar Projectile")]
    [SerializeField] private GameObject AmmoPrefab;

    [Header("Laser Attributes")]
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float startingWidth;
    [SerializeField] private float endingWidth;
    private LineRenderer laserRenderer;

    private Vector3 targetPos;
    private bool userKilled;
    private bool targetingActive;

    //Interface
    public event EventHandler OnParticleCall;

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
        userKilled = false;

        targetingActive = false;
        timeUntilShoot = 0.0f;

        targetPos = new Vector3(0, 15, 0);

        MortarBody = GetComponent<DestroyableObject>();
        MortarBody.OnDestroyed += MortarDestroyed;
        

        SpawnEnemy();

        if (enBehav)
        {
            enBehav.OnDeath += OnUserKilled;
            enBehav.MovementAnimator.SetBool("Crouching", true);
        }
    }

    private void OnUserKilled(object sender, EventArgs e)
    {
        userKilled = true;
        enBehav.OnDeath -= OnUserKilled;
    }

    private void MortarDestroyed(object sender, EventArgs e)
    {
        ToggleEnemyBehavior(true);
    }

    void Update()
    {
        if (enabled)
        {
            if (userKilled || Physics.CheckSphere(gameObject.transform.position, radius, playerMask))
            {
                ToggleEnemyBehavior(true);

                this.enabled = false;
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

        yield return new WaitForSeconds(targetingDelay);
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
        if (enabled)
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
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            
        }
    }
    private void ShootMortar()
    {
        OnParticleCall?.Invoke(this, EventArgs.Empty);
        Instantiate(AmmoPrefab, targetPos, Quaternion.Euler(90, 0, 0));
    }
    private void SpawnEnemy()
    {
        enemy = Instantiate(heldEnemy, mountPoint.position, mountPoint.rotation);
        enBehav = enemy.GetComponent<EnemyBehavior>();
        ToggleEnemyBehavior(false);
    }

    private void ToggleEnemyBehavior(bool active)
    {
        if(enBehav)
        {
            enBehav.TargetingEnabled = active;
            enBehav.MovementAnimator.SetBool("Crouching", !active);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}
