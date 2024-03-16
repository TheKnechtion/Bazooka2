using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [Tooltip("Toggle this if you want this Respawner to manage a single enemy instead of constantly respawning")]
    [SerializeField] private bool ManageSingleEnemy;
    private bool SpawningEnemy;
    private GameObject ManagedEnemy;
    void Start()
    {
        SpawnEnemy();
    }
    void Update()
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

    private void SpawnEnemy()
    {
        //First check if list has enemies to spawn
        if (PossibleEnemies.Length>0)
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
}
