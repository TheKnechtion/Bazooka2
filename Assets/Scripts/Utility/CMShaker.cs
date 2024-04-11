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
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.F))
        {
            ShakeCam();
        }
    }

    private void ShakeCam()
    {
        StartCoroutine(ShakeRoutine(1.3f, 0.03f));
    }

    private IEnumerator ShakeRoutine(float shakeTime, float max)
    {
        float t = 0.0f;
        Vector3 basePos = transform.position;

        while (t < shakeTime)
        {
            float x = UnityEngine.Random.Range(-max, max);
            float y = UnityEngine.Random.Range(-max, max);

            transform.position = new Vector3(transform.position.x + x,
                transform.position.y + y, 
                transform.position.z);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = basePos;
        yield return null;
    }
}
