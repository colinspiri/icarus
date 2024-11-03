using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] source;
    private int channel = -1;

    public static AudioManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this) 
        { 
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip, float volume)
    {
        channel++;
        if (channel == source.Length)
        {
            channel = 0;
        }
        source[channel].clip = clip;
        source[channel].volume = volume;
        source[channel].Play();
    }
}
