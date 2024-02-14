using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathScipt : MonoBehaviour
{
    [Header("Rewards Given")]
    [Tooltip("Put would rewards you want this boss to drop when defeated.")]
    [SerializeField] private GameObject[] rewards;

    [Header("Destoyed Model")]
    [SerializeField] private GameObject destoyedVersion;
    private GameObject destroyedObj;

    [Header("Objects to disable")]
    [Tooltip("Some objects are composed of multiple gameobjects, so add them here to be disabled when killed.")]
    [SerializeField] private GameObject[] bodyObjects;

    // Start is called before the first frame update
    void Start()
    {
        destroyedObj = Instantiate(destoyedVersion, gameObject.transform.position, Quaternion.identity);
        destroyedObj.SetActive(false);

        //Checks for different Boss Behaviors
        //  TODO: Spider, Robot
        if (gameObject.TryGetComponent<BehaviorTankBoss>(out BehaviorTankBoss t))
        {
            t.InstanceTankKilled += OnThisKilled;
        }
    }

    private void OnThisKilled(object sender, System.EventArgs e)
    {
        if (destroyedObj != null)
        {
            DisableBody(bodyObjects);
            destroyedObj.transform.position = gameObject.transform.position;
            destroyedObj.transform.rotation = gameObject.transform.rotation;
            destroyedObj.SetActive(true);
        }

        if (rewards.Length > 0)
        {
            SpawnRewards(rewards);
        }

        Destroy(gameObject);
    }

    private void SpawnRewards(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            GameObject temp = Instantiate(array[i], gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    private void DisableBody(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i].SetActive(false);
        }
    }

}
