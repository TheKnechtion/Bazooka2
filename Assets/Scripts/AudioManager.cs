using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip themeClip;

    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip bazookaSound;
    static AudioClip bazookaSoundRef;
    static AudioClip explosionSoundRef;

    AudioSource audioSource;
    public float weaponsVolume = 0.5f;
    public float themeVolume = 0.01f;
    static float weaponsVolumeRef;
    
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayTheme();
        bazookaSoundRef = bazookaSound;
        explosionSoundRef = explosionSound;
    }

    private void Update()
    {
        weaponsVolumeRef = weaponsVolume;
    }
    public void PlayTheme()
    {
        audioSource.PlayOneShot(themeClip, themeVolume);
        audioSource.PlayScheduled(AudioSettings.dspTime + themeClip.length);
    }

    static AudioClip clipToPlay;

    public static void PlayClipAtPosition(string clip, Vector3 position)
    {
        switch(clip)
        {
            case "bazooka_fire":
                clipToPlay = bazookaSoundRef;
                break;

            case "explosion_sound":
                clipToPlay = explosionSoundRef;
                break;

            default:
                clipToPlay = explosionSoundRef;
                break;
        }
        AudioSource.PlayClipAtPoint(clipToPlay, position, weaponsVolumeRef);
    }

}
