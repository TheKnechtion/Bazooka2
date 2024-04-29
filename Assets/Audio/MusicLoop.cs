using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicLoop : MonoBehaviour
{
    public Sound Music_Intro;
    public Sound Music_Loop;
    public Sound Music_Outro;

    private AudioSource IntroSource;
    private AudioSource LoopSource;
    private AudioSource OutroSource;

    public bool BossMusic;

    [SerializeField] private bool StartOnAwake;
    private bool MusicPlaying;

    private void Awake()
    {
        IntroSource = CreateSource(Music_Intro);
        LoopSource = CreateSource(Music_Loop);
        OutroSource = CreateSource(Music_Outro);
    }

    private void Start()
    {
        /*
        if (StartOnAwake)
        {
            StartMusic();
        }
        
        if (BossMusic)
        {
            BehaviorTankBoss.OnCaughtAggro += StartingEvents;
            BehaviorTankBoss.OnTankKilled += StoppingEvents;
        }
        */
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

    public void StartMusic()
    {
        StopAllCoroutines();

        StartCoroutine(StartMusicCoroutine());
    }

    public void StopMusic(bool ReturnToBase)
    {
        StopAllCoroutines();
        StartCoroutine(StopMusicCoroutine(ReturnToBase));
    }

    private IEnumerator StartMusicCoroutine()
    {
        MusicPlaying = true;

        GameObject gm = GameObject.Find("GameManager");
        if (gm != null)
        {
            gm.GetComponent<AudioManager>().StopTheme();
        }

        if (IntroSource != null)
        {
            IntroSource.Play();

            while (IntroSource.isPlaying)
            {
                yield return null;
            }
        }

        

        if (LoopSource != null)
        {
            LoopSource.Play();
        }

        yield return null;
    }

    private IEnumerator StopMusicCoroutine(bool ReturnBaseMusic)
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

        if (ReturnBaseMusic)
        {
            GameObject gm = GameObject.Find("GameManager");
            if (gm != null)
            {
                gm.GetComponent<AudioManager>().PlayTheme();
            }
        }

        yield return null;
    }
}
