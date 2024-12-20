using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] source;
    private int channel = 0;

    public static AudioManager Instance = null;

    [SerializeField] private AudioSettings audioSettings;

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

    private void Start() {
        audioSettings.Initialize();
    }

    public void Play(AudioClip clip, float volume)
    {
        int prev = channel;
        while (source[channel].isPlaying)
        {
            channel++;
            if (channel == source.Length)
            {
                channel = 0;
            }
            if (channel == prev)
            {
                break;
            }
        }
        source[channel].clip = clip;
        source[channel].volume = volume;
        source[channel].Play();
    }
}
