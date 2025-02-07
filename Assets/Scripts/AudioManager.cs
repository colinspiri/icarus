using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> source = new List<AudioSource>();

    public static AudioManager Instance = null;

    [SerializeField] private AudioSettings audioSettings;
    [SerializeField] private AudioMixer audioMixer;

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

    public void Play(AudioClip clip, float volume, float pitch, AudioMixerGroup mixer)
    {
        List<AudioSource> toDelete = new List<AudioSource>();
        foreach (AudioSource s in source)
        {
            if (!s.isPlaying)
            {
                toDelete.Add(s);
            }
        }
        foreach (AudioSource s in toDelete)
        {
            source.Remove(s);
            Destroy(s);
        }
        AudioSource audio = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        source.Add(audio);
        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;
        audio.outputAudioMixerGroup = mixer;
        audio.Play();
    }

    public void Play(AudioClip clip, float volume)
    {
        Play(clip, volume, 1.0f, audioMixer.FindMatchingGroups("Master")[0]);
    }
}
