using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SpawnInvoker {TRIGGER, AUTOMATIC}
public class EnemyRespawner : MonoBehaviour
{
    /// <summary>
    /// Array of possible enemies that can spawn here, only put 1 if you 
    /// want 1 enemy type to spawn from this Spawner
    /// </summary>
    [SerializeField] private GameObject[] PossibleEnemies;

    /// <summary>
    /// Delay between each spawn when invoked
    /// </summary>
    [SerializeField] private float SpawnTimer;

    /// <summary>
    /// This can be used for special case enemies, if you want them to respawn after a certain amount
    /// or to manage special type enemies that 
    /// </summary>
    /// 
    [Tooltip("Toggling this will spawn 1 enemy that will respawn when killed, does not Spawn more than 1 at a time")]
    [SerializeField] private bool ManageSingleEnemy;
    private bool SpawningEnemy;
    private GameObject ManagedEnemy;

    /// <summary>
    /// Set the requirement to trigger the spawning. Automatic means that this spawns from the start. 
    /// Trigger is for spawning in when you want, requires 'TriggerEvent' object to connect UnityEvent.
    /// </summary>
    [Tooltip("Set the requirement to trigger the spawning." +
        "\n\nAUTOMATIC: Spawns enemy from start, no requirement." +
        "\n\nTRIGGER: Spawns/Activates in when you want, requires 'TriggerEvent' object to connect UnityEvent." +
        "\n\nNOTE: Do not use AUTOMATIC when this is connected to a Trigger Event, you must remove the Trigger Event")]
    [SerializeField] private SpawnInvoker SpawnType;

    void Start()
    {

    }
    void Update()
    {
        switch (SpawnType)
        {
            case SpawnInvoker.TRIGGER:


                break;
            case SpawnInvoker.AUTOMATIC:
                HandleEnemySpawning();

                break;
            default:
                break;
        }
        
    }

    public void HandleEnemySpawning()
    {
        if (ManageSingleEnemy)
        {
            CheckEnemyReference();
        }
        else
        {
            if (!SpawningEnemy)
            {
                StartCoroutine(SpawnAfterTimer());
            }
        }
    }
    private void CheckEnemyReference()
    {
        if (ManagedEnemy == null)
        {
            if (!SpawningEnemy)
            {
                StartCoroutine(SpawnAfterTimer());
            }
        }
    }

    private IEnumerator SpawnAfterTimer()
    {
        SpawningEnemy = true;
        float t = 0.0f;
        while (t < SpawnTimer)
        {
            t += Time.deltaTime;
            yield return null;
        }

        SpawnEnemy();
        SpawningEnemy = false;
        yield return null;
    }
    private void SpawnEnemy()
    {
        //First check if list has enemies to spawn
        if (PossibleEnemies.Length > 0)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                if (PossibleEnemies.Length < 2)
                {
                    GameObject temp = Instantiate(PossibleEnemies[0], hit.position, Quaternion.identity);
                    ManagedEnemy = temp;
                }
                else
                {
                    int randomIndex = Random.Range(0, PossibleEnemies.Length + 1);
                    GameObject temp = Instantiate(PossibleEnemies[randomIndex], transform.position, Quaternion.identity);
                }
            }
        }
    }
}
