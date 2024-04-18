using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingObjSpawner : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] private GameObject[] PossibleObjects;

    [Tooltip("Activate an object every 'x' amount of seconds.")]
    [Range(0f, 4.0f)]
    [SerializeField] private float SpawnDelay;
    private float currentSpawnTime;

    [Tooltip("Objects will be active for 'this' amount of time, seconds.")]
    [Range(1, 15)]
    [SerializeField] private float ObjectLifeTime;

    [Tooltip("How many instances of each object there will be stored.")]
    [Range(1, 5)]
    [SerializeField] private int InstancesPerObj;

    private Stack<GameObject> SpawnStack;   
    private Queue<GameObject> ResetQueue;

    void Start()
    {
        SpawnStack = new Stack<GameObject>();
        ResetQueue= new Queue<GameObject>();

        currentSpawnTime = 0.0f;

        for (int i = 0; i < PossibleObjects.Length; i++)
        {
            for (int n = 0; n < InstancesPerObj; n++)
            {
                //GameObject var = Instantiate(PossibleObjects[i], gameObject.transform.position, Quaternion.identity);
                GameObject var = Instantiate(PossibleObjects[i], transform);
                var.SetActive(false);

                SpawnStack.Push(var);   
            }
        }
    }

    void Update()
    {
        if (ResetQueue.Count > 0)
        {
            SpawnStack.Push(ResetQueue.Dequeue());
        }

        if (currentSpawnTime < SpawnDelay)
        {
            currentSpawnTime += Time.deltaTime;
        }
        else
        {
            currentSpawnTime = 0.0f;
            ActivateObject();
        }
    }

    public void ActivateObject()
    {
        GameObject var;
        if (SpawnStack.Count > 0)
        {
            var = SpawnStack.Pop();
            var.SetActive(true);
        }
        else
        {
            int random = Random.Range(0, PossibleObjects.Length-1);
            var = Instantiate(PossibleObjects[random], transform);
        }        

        var.gameObject.transform.position = gameObject.transform.position;

        StartCoroutine(ResetIntoStack(var, ObjectLifeTime));
    }

    private void DeativateAndReset(GameObject passedObj)
    {
        passedObj.SetActive(false);
        ResetQueue.Enqueue(passedObj);
    }

    private IEnumerator ResetIntoStack(GameObject passedObj, float time)
    {
        float t = 0.0f;
        while (t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }

        DeativateAndReset(passedObj);

        yield return null;
    }
}
