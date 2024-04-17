using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResusableAudioController : MonoBehaviour
{
    /// <summary>
    /// Array of all audio clips this Object can play
    /// </summary>
    [SerializeField] private Sound[] Clips;

    /// <summary>
    /// Dictionanary used for AudioSource look-up
    /// </summary>
    private Dictionary<string, AudioSource> SoundDictionary;

    /// <summary>
    /// Automatically plays first clip in Array
    /// </summary>
    [SerializeField] private bool PlayFirstClip;

    private string MainClip;

    private void Awake()
    {
        if (Clips != null && Clips.Length > 0)
        {
            MainClip = Clips[0].Name;
            SoundDictionary = new Dictionary<string, AudioSource>();
            for (int i = 0; i < Clips.Length; i++)
            {
                AudioSource a = gameObject.AddComponent<AudioSource>();
                a.clip = Clips[i].clip;
                a.volume = Clips[i].Volume;
                a.spatialBlend = 1;
                a.loop = Clips[i].Loop;
                SoundDictionary.Add(Clips[i].Name, a);
            }
        }
    }

    private void Start()
    {
        if (PlayFirstClip)
        {
            SoundDictionary[MainClip].Play();
        }
    }

    public void PlaySound(string clipName)
    {
        if (SoundDictionary.ContainsKey(clipName))
        {
            if (!SoundDictionary[clipName].isPlaying)
            {
                SoundDictionary[clipName].Play();
            }
        }
    }

    public void StopSound(string clipName)
    {
        if (SoundDictionary.ContainsKey(clipName))
        {
            SoundDictionary[clipName].Stop();
        }
    }

}
