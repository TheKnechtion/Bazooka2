using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalPool : MonoBehaviour
{
    [Tooltip("Decal Prefabs")]
    [SerializeField] private GameObject[] ExplosionDecals;
    [SerializeField] private int InstancePerDecal;

    private Stack<GameObject> DecalStack;
    private Queue<GameObject> ResetQueue;
    void Start()
    {
        DontDestroyOnLoad(this);
        DecalStack = new Stack<GameObject>();
        ResetQueue = new Queue<GameObject>();

        if (ExplosionDecals.Length>0)
        {
            for (int i = 0; i < InstancePerDecal; i++)
            {
                for (int n = 0; n < ExplosionDecals.Length; n++)
                {
                    GameObject newDecal = Instantiate(ExplosionDecals[n], transform);
                    DecalStack.Push(newDecal);
                    newDecal.SetActive(false);  
                }
            }
        }

        DestroyProjectile.ProjectileDestroyed += DestroyProjectile_OnDestroy;
    }

    private void DestroyProjectile_OnDestroy(object sender, System.Tuple<Vector3, Quaternion, Transform> e)
    {
        GetDecal(e.Item1, e.Item2, e.Item3);
    }

    private void Update()
    {
        if (ResetQueue.Count > 0)
        {
            DecalStack.Push(ResetQueue.Dequeue());
        }
    }
    private void GetDecal(Vector3 position, Quaternion lookRotation, Transform parent)
    {
        if (DecalStack.Count > 0)
        {
            if (position != null)
            {
                GameObject decal = DecalStack.Pop();
                decal.transform.position = position;
                decal.transform.rotation = lookRotation;
                decal.transform.SetParent(parent, true);
                decal.SetActive(true);

                StartCoroutine(ResetIntoStack(decal, 3));
            }
        }
    }
    private void DeativateAndReset(GameObject passedObj)
    {
        if (passedObj != null)
        {
            passedObj.SetActive(false);
            passedObj.transform.SetParent(transform);
            ResetQueue.Enqueue(passedObj);
        }        
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
        DestroyProjectile.ProjectileDestroyed -= DestroyProjectile_OnDestroy;
    }
}
