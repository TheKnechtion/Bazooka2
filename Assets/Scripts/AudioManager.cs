using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip themeClip;

    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip bazookaSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip clickSound;

    static AudioClip bazookaSoundRef;
    static AudioClip explosionSoundRef;
    static AudioClip hitSoundRef;
    static AudioClip engineSoundRef;
    static AudioClip clickSoundRef;

    [SerializeField] AudioClip[] painSounds;
    static AudioClip[] painSoundsRef;

    AudioSource audioSource;
    public float weaponsVolume = 0.5f;
    public float themeVolume = 0.01f;
    public float painVolume = 0.75f;
    static float weaponsVolumeRef;
    static float painVolumeRef;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayTheme();
        bazookaSoundRef = bazookaSound;
        explosionSoundRef = explosionSound;
        hitSoundRef = hitSound;
        clickSoundRef=clickSound;
        engineSoundRef = engineSound;
        painSoundsRef=painSounds;
    }

    float loopClipTime;
    private void Update()
    {
        weaponsVolumeRef = weaponsVolume;

        painVolumeRef = painVolume;
    }
    public void PlayTheme()
    {
        audioSource.clip = themeClip;
        audioSource.loop = true;
        audioSource.volume = themeVolume;
        audioSource.Play();
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

            case "hit_sound":
                clipToPlay = hitSoundRef;
                break;

            case "engine_sound":
                clipToPlay = engineSoundRef;
                break;

            case "click":
                clipToPlay = clickSoundRef;
                break;

            default:
                clipToPlay = explosionSoundRef;
                break;
        }

        if (clipToPlay != null)
        {
            AudioSource.PlayClipAtPoint(clipToPlay, position, weaponsVolumeRef);
        }
    }

    public static void PlayPainClipAtPosition(Vector3 position)
    {
        clipToPlay = painSoundsRef[UnityEngine.Random.Range(0, painSoundsRef.Length - 1)];

        AudioSource.PlayClipAtPoint(clipToPlay, position, painVolumeRef);
    }



}
