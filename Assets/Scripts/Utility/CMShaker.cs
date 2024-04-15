using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMShaker : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    //Used to create impulses
    private CinemachineImpulseSource noiseSource;
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        noiseComponent = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseSource = cam.GetComponent<CinemachineImpulseSource>();

        Explosive.OnExploded += CameraShake;
        PlayerInfo.GlobalDamge += CameraShake;
    }

    private void CameraShake(object sender, System.EventArgs e)
    {
        ShakeCam();
    }
    private void ShakeCam()
    {
        StartCoroutine(ShakeRoutine(0.3f, 2.5f));
    }

    private IEnumerator ShakeRoutine(float shakeTime, float max)
    {
        Debug.Log("Shaking");
        float t = 0.0f;
        noiseComponent.m_AmplitudeGain = max;

        while (t < shakeTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        noiseComponent.m_AmplitudeGain = 0.0f;

        yield return null;
    }
    private void OnDestroy()
    {
        Explosive.OnExploded -= CameraShake;
        PlayerInfo.GlobalDamge -= CameraShake;
    }
}
