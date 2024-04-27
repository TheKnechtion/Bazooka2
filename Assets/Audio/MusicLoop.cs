using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicLoop : MonoBehaviour
{
    [SerializeField] private Sound Music_Intro;
    [SerializeField] private Sound Music_Loop;
    [SerializeField] private Sound Music_Outro;

    private AudioSource IntroSource;
    private AudioSource LoopSource;
    private AudioSource OutroSource;

    [SerializeField] private bool StartOnAwake;
    private bool MusicPlaying;

    private void Awake()
    {
        IntroSource = CreateSource(Music_Intro);
        LoopSource = CreateSource(Music_Loop);
        //OutroSource = CreateSource(Music_Outro);
    }

    private void Start()
    {
        if (StartOnAwake)
        {
            StartMusic();
        }
    }

    private AudioSource CreateSource(Sound newSound)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.clip = newSound.clip;
        a.volume = newSound.Volume;
        a.spatialBlend = newSound.SpatialBlend;
        a.loop = newSound.Loop;
        a.maxDistance = newSound.MaxRange;
        a.spatialBlend = newSound.SpatialBlend;

        return a;
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(StartMusicCoroutine());
            //LoopSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine());
        }
    }

    public void StartMusic()
    {
        StartCoroutine(StartMusicCoroutine());
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        StartCoroutine(StopMusicCoroutine());
    }

    private IEnumerator StartMusicCoroutine()
    {
        MusicPlaying = true;
        GameObject.Find("GameManager").GetComponent<AudioManager>().StopTheme();

        if (IntroSource != null)
        {
            IntroSource.Play();
        }

        while (IntroSource.isPlaying)
        {
            yield return null;
        }

        if (LoopSource != null)
        {
            LoopSource.Play();
        }

        yield return null;
    }

    private IEnumerator StopMusicCoroutine()
    {
        MusicPlaying = false;

        IntroSource.Stop();
        LoopSource.Stop();

        if (OutroSource != null)
        {
            OutroSource.Play();
            while (OutroSource.isPlaying)
            {
                yield return null;
            }
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<AudioManager>().PlayTheme();
        }

        yield return null;
    }

}
