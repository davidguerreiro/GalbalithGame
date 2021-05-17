using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip mainLevelTheme;
    public AudioClip battleTheme;

    [Header("Settings")]
    public bool playAtInit;

    [HideInInspector]
    public AudioComponent audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        if (playAtInit) {
            PlayMainLevelSong();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Play song for this level.
    /// </summary>
    /// <param name="songName">AudioClip - song name to play</param>
    public void PlayLevelSong(AudioClip clip)
    {
       audio.PlayClip(clip);
    }

    /// <summary>
    /// Play main level song.
    /// </summary>
    public void PlayMainLevelSong()
    {
        audio.PlayClip(mainLevelTheme);
    }

    /// <summary>
    /// Play battle theme song.
    /// </summary>
    public void PlayBattleTheme()
    {
        audio.PlayClip(battleTheme);
    }

    /// <summary>
    /// Stop level song.
    /// </summary>
    public void StopLevelSong()
    {
        audio.StopAudio();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        audio = GetComponent<AudioComponent>();
    }
}
