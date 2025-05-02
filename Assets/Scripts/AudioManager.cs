using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> source = new List<AudioSource>();

    public static AudioManager Instance = null;

    [SerializeField] private AudioSettings audioSettings;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SoundProfile staticLoop;
    [SerializeField] private SoundProfile radioStart;
    [SerializeField] private SoundProfile radioStop;

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

    public void Play(AudioClip clip, float volume, float pitch, AudioMixerGroup mixer, bool looping = false)
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
        audio.loop = looping;
        audio.Play();
    }

    public void StopSFX(AudioClip clip)
    {
        foreach (AudioSource s in source)
        {
            if (clip == s.clip)
            {
                s.Stop();
            }
        }
    }

    public void Play(AudioClip clip, float volume)
    {
        Play(clip, volume, 1.0f, audioMixer.FindMatchingGroups("Master")[0]);
    }

    [YarnCommand("BeginRadio")]
    public void BeginRadio()
    {
        Debug.Log("BeginRadio");
        radioStart.PlaySFX();
    }

    [YarnCommand("EndRadio")]
    public void EndRadio()
    {
        radioStop.PlaySFX();
    }

    [YarnCommand("StartRadioStatic")]
    public void StartRadioStatic()
    {
        staticLoop.PlaySFX();
    }

    [YarnCommand("EndRadioStatic")]
    public void EndRadioStatic()
    {
        staticLoop.StopSFX();
    }
}
