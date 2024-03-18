using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DecalPool : MonoBehaviour
{
    [Tooltip("Decal Prefabs")]
    [SerializeField] private GameObject[] ExplosionDecals;
    [SerializeField] private int InstancePerDecal;

    private Stack<GameObject> DecalStack;
    private Queue<GameObject> ResetQueue;
    void Start()
    {
        DecalStack = new Stack<GameObject>();
        ResetQueue = new Queue<GameObject>();

        if (ExplosionDecals.Length>0)
        {
            for (int i = 0; i < InstancePerDecal; i++)
            {
                for (int n = 0; n < ExplosionDecals.Length; n++)
                {
                    GameObject newDecal = Instantiate(ExplosionDecals[n]);
                    DecalStack.Push(newDecal);
                    newDecal.SetActive(false);  
                }
            }
        }

        DestroyProjectile.OnDestroy += DestroyProjectile_OnDestroy;
    }

    private void DestroyProjectile_OnDestroy(object sender, System.Tuple<Vector3, Quaternion> e)
    {
        GetDecal(e.Item1, e.Item2);
    }

    private void Update()
    {
        if (ResetQueue.Count > 0)
        {
            DecalStack.Push(ResetQueue.Dequeue());
        }
    }
    private void GetDecal(Vector3 position, Quaternion lookRotation)
    {
        if (DecalStack.Count > 0)
        {
            GameObject decal = DecalStack.Pop();
            decal.transform.position = position;
            decal.transform.rotation = lookRotation;
            decal.SetActive(true);

            StartCoroutine(ResetIntoStack(decal, 3));
        }
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

    private void OnDestroy()
    {
        DestroyProjectile.OnDestroy -= DestroyProjectile_OnDestroy;
    }
}
