using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDecorator : MonoBehaviour
{
    [SerializeField] private float Radius;
    [SerializeField] private int Damage;
    [SerializeField] private GameObject BarrelPrefab;
    [Tooltip("How long (seconds) it takes to spawn a new barrel")]
    [Range(0.5f, 10.0f)]
    [SerializeField] private float SpawnDelay;

    private Explosive heldExplosive;
    private GameObject BarrelRef;

    private bool CreatingBarrel;

    // Start is called before the first frame update
    void Start()
    {
        CreatingBarrel = false;
        CreateDecoratedBarrel();   
    }

    // Update is called once per frame
    void Update()
    {
        if (BarrelRef == null && !CreatingBarrel)
        {
            StartCoroutine(SpawnBarrel(SpawnDelay));
        }
    }

    private void CreateDecoratedBarrel()
    {
        BarrelRef = Instantiate(BarrelPrefab, transform.position, transform.rotation);
        heldExplosive = BarrelRef.GetComponent<Explosive>();
        if (heldExplosive != null)
        {
            heldExplosive.ExplosionRadius = Radius;
            heldExplosive.Damage = Damage;
        }
    }

    private IEnumerator SpawnBarrel(float time)
    {
        CreatingBarrel = true;

        float t = 0.0f;
        while (t < time)
        {
            t += Time.deltaTime;
            yield return null; 
        }

        CreateDecoratedBarrel();

        CreatingBarrel = false;

        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
