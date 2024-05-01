using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class FullScreenVFXController : MonoBehaviour
{
    [SerializeField] private VignetteSO DamageVFX;
    [SerializeField] private VignetteSO CheckpointVFX;

    [SerializeField] private float QuickTransition = 0.3f;
    [SerializeField] private float TransitionTime = 1.3f;

    [Header("Render Feature")]
    [SerializeField] private ScriptableRendererFeature FullScreenEffect;
    [SerializeField]private Material FullScreenMaterial;

    private bool DamageActive;

    //Singleton implementation
    public static FullScreenVFXController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        if (FullScreenEffect == null || FullScreenMaterial == null)
        {
            Debug.LogWarning("! FullScreen VFX paremeter not set !");
        }

        QuickReset();
    }
    public void SetDamageEffect()
    {
        DamageActive = true;
        StartCoroutine(SwapEffect(DamageVFX, false));
    }

    public void SetCheckpointEffect()
    {
        StartCoroutine(SwapEffect(CheckpointVFX, DamageActive));
    }

    public void ResetEffect()
    {
        DamageActive = false;
        StartCoroutine(EffectReset());
    }

    private void QuickReset()
    {
        FullScreenMaterial.SetFloat("_VigIntensity", 0);
    }

    #region Coroutines
    private IEnumerator SwapEffect(VignetteSO effect, bool returnToDamage)
    {
        FullScreenMaterial.SetFloat("_VigPower", effect.VignettePower);
        FullScreenMaterial.SetColor("_VigColor", effect.color);
        FullScreenMaterial.SetFloat("_NoiseSpeed", effect.NoiseSpeed);
        FullScreenMaterial.SetFloat("_NoisePower", effect.NoisePower);
        FullScreenMaterial.SetFloat("_NoiseIntensity", effect.NoiseIntensity);

        float t = 0.0f;
        while (t < QuickTransition)
        {
            float intensity = Mathf.Lerp(0, effect.VignetteInstensity, t);
            FullScreenMaterial.SetFloat("_VigIntensity", intensity);

            t += Time.deltaTime;
            yield return null;
        }

        FullScreenMaterial.SetFloat("_VigIntensity", effect.VignetteInstensity);
        

        if (effect.Transitions)
        {
            t = 0.0f;
            while(t < TransitionTime)
            {
                float intensity = Mathf.Lerp(effect.VignetteInstensity, 0, t);
                FullScreenMaterial.SetFloat("_VigIntensity", intensity);

                t += Time.deltaTime;
                yield return null;
            }

            if (returnToDamage)
            {
                SetDamageEffect();
            }
        }

        yield return null;
    }

    private IEnumerator EffectReset()
    {
        float start = FullScreenMaterial.GetFloat("_VigIntensity");

        float t = 0.0f;
        while(t < TransitionTime)
        {
            start = Mathf.Lerp(start, 0, t);

            FullScreenMaterial.SetFloat("_VigIntensity", start);

            t += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    #endregion
}
