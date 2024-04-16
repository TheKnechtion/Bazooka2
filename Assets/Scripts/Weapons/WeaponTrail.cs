using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    //THIS trail Renderer
    private TrailRenderer trailRenderer;

    //Bullet this is attatched to
    private DestroyProjectile Evoker;

    [Range(0f, 3f)]
    [SerializeField] private float DissipateTime = 1.0f;

    private bool FullyDisipated;
    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        if (gameObject.transform.parent.TryGetComponent<DestroyProjectile>(out DestroyProjectile t))
        {
            Evoker = t;
            Evoker.InstanceDetroyed += BulletDestroyed;
        }
    }

    private void BulletDestroyed(object sender, System.EventArgs e)
    {
        transform.SetParent(null);

        if (trailRenderer != null)
        {
            StartCoroutine(DissipateTrail(DissipateTime));
        }
    }

    private void Update()
    {
        if (FullyDisipated)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DissipateTrail(float dissipateTime)
    {
        Material trailMat = trailRenderer.material;

        float t = 0.0f;
        float opac = trailMat.GetFloat("_Opacity");
        while (t < dissipateTime)
        {
            opac = Mathf.Lerp(opac, 0, t/dissipateTime);
            trailMat.SetFloat("_Opacity", opac);

            t += Time.deltaTime;
            yield return null;
        }

        trailMat.SetFloat("_Opacity", 0);

        FullyDisipated = true;
        yield return null;
    }

}
