using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private MusicLoop SceneMusic;
    [SerializeField] private MusicLoop ExtraMusic;
    private MusicLoop CurrentMusic;

    private void Awake()
    {
        BehaviorTankBoss.OnCaughtAggro += ExtraStartInvoker;
        BehaviorTankBoss.OnTankKilled += ExtraStopInvoker;
    }
    private void ExtraStartInvoker(object sender, System.EventArgs e)
    {
        SwitchMusic(true);
    }
    private void ExtraStopInvoker(object sender, System.EventArgs e)
    {
        SwitchMusic(false);
    }
    private void Start()
    {
        if (SceneMusic != null)
        {
            CurrentMusic = SceneMusic;
            CurrentMusic.StartMusic();
        }
    }

    public void SwitchMusic(bool Extra)
    {
        if (Extra)
        {
            if (ExtraMusic != null && ExtraMusic.Music_Loop != null)
            {
                CurrentMusic.StopMusic(false);

                CurrentMusic = ExtraMusic;
                CurrentMusic.StartMusic();
            }            
        }
        else
        {
            CurrentMusic.StopMusic(false);
            CurrentMusic = SceneMusic;
            CurrentMusic.StartMusic();
        }
    }

    public void ReturnToBaseMusic()
    {
        CurrentMusic.StopMusic(true);
    }

    private void OnDestroy()
    {
        BehaviorTankBoss.OnCaughtAggro -= ExtraStartInvoker;
        BehaviorTankBoss.OnTankKilled -= ExtraStopInvoker;
    }
}
