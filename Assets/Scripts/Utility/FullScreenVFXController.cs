using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class FullScreenVFXController : MonoBehaviour
{
    [SerializeField] private Color DamageColor;
    [SerializeField] private Color CheckpointColor;

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


}
